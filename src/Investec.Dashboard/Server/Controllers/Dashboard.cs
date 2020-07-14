using Investec.Dashboard.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Investec.Dashboard.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Dashboard : ControllerBase
    {
        private static readonly string[] Summaries = new[]
 {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly InvestecDBContext _investecDB;

        public Dashboard(InvestecDBContext investecDB)
        {
            _investecDB = investecDB;
        }

        [HttpGet]
        [Route("LastThirtyDaysSpend")]
        public async Task<IEnumerable<TransactionDay>> LastThirtyDaysSpend()
        {
            List<TransactionDay> transactionDays = new List<TransactionDay>();

            var lastThirtyDays = await _investecDB.Transactions.Where(t => t.Type.Equals("DEBIT") & t.PostingDate > DateTime.UtcNow.AddDays(-30)).OrderBy(t => t.PostingDate).ToListAsync();
            var lastThirtyDaysGroupedByMonth = lastThirtyDays.GroupBy(t => t.PostingDate.Month);

            foreach (var transactionGroup in lastThirtyDaysGroupedByMonth)
            {
                var groupedByDay = transactionGroup.GroupBy(tg => tg.PostingDate.Day);

                foreach (var transactionDay in groupedByDay)
                {
                    transactionDays.Add(new TransactionDay
                    {
                        Date = transactionDay.First().PostingDate,
                        Type = "DEBIT",
                        Amount = (double)transactionDay.Sum(td => td.Amount)
                    });
                }
            }

            return transactionDays;
        }
    }
}