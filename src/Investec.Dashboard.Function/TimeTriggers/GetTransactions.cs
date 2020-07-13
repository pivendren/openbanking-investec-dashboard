using Investec.Dashboard.Shared.Entities;
using Investec.Dashboard.Shared.Models.OpenAPI;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Investec.Dashboard.Function.TimeTriggers
{
    public class GetTransactions
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly InvestecDBContext _context;

        public GetTransactions(IHttpClientFactory clientFactory, InvestecDBContext context)
        {
            _clientFactory = clientFactory;
            _context = context;
        }

        [FunctionName("GetTransactions")]
        public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
        {
            try
            {
                var client = _clientFactory.CreateClient();

                var dict = new Dictionary<string, string>();
                dict.Add("grant_type", "client_credentials");
                dict.Add("scope", "accounts");

                var clientCreds = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes($"{Environment.GetEnvironmentVariable("ClientId")}:{Environment.GetEnvironmentVariable("ClientSecret")}"));

                var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://openapi.investec.com/identity/v2/oauth2/token") { Content = new FormUrlEncodedContent(dict) };
                tokenRequest.Headers.Add("Authorization", $"Basic {clientCreds}");

                var tokenResponse = await client.SendAsync(tokenRequest).Result.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<TokenResponseModel>(tokenResponse);

                var transactionRequest = new HttpRequestMessage(HttpMethod.Get, $"https://openapi.investec.com/za/pb/v1/accounts/" + Environment.GetEnvironmentVariable("AccountId") + "/transactions");
                transactionRequest.Headers.Add("Authorization", $"Bearer {token.Access_token}");
                transactionRequest.Headers.Add("User-Agent", "AzureFunction");
                var transactionResponse = await client.SendAsync(transactionRequest).Result.Content.ReadAsStringAsync();
                var transactions = JsonConvert.DeserializeObject<TransactionsResponseModel>(transactionResponse).Data.Transactions;

                var t = transactions.First();

                _context.Add<TransactionEntity>(new TransactionEntity
                {
                    AccountId = t.AccountId,
                    ActionDate = t.ActionDate,
                    Amount = t.Amount,
                    CardNumber = t.CardNumber,
                    Description = t.Description,
                    PostingDate = t.PostingDate,
                    Status = t.Status,
                    Type = t.Type,
                    ValueDate = t.ValueDate
                });
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}