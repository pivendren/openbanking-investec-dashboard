using Newtonsoft.Json;
using System;

namespace Investec.Dashboard.Shared.Models.OpenAPI
{
    public class TransactionsResponseModel
    {
        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("links")]
        public Links Links { get; set; }

        [JsonProperty("meta")]
        public Meta Meta { get; set; }
    }

    public class Data
    {
        [JsonProperty("transactions")]
        public Transaction[] Transactions { get; set; }
    }

    public class Transaction
    {
        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("cardNumber")]
        public string CardNumber { get; set; }

        [JsonProperty("postingDate")]
        public DateTime PostingDate { get; set; }

        [JsonProperty("valueDate")]
        public DateTime ValueDate { get; set; }

        [JsonProperty("actionDate")]
        public DateTime ActionDate { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }
    }

    public class Links
    {
        [JsonProperty("self")]
        public string Self { get; set; }
    }

    public class Meta
    {
        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }
    }
}