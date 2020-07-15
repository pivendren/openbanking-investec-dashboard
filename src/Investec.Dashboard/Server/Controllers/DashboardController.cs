using Investec.Dashboard.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Investec.Dashboard.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly InvestecDBContext _investecDB;

        public DashboardController(InvestecDBContext investecDB)
        {
            _investecDB = investecDB;
        }

        [HttpGet]
        [Route("LastThirtyDaysSpend")]
        public async Task<IEnumerable<TransactionDay>> LastThirtyDaysSpend()
        {
            List<TransactionDay> transactionDays = new List<TransactionDay>();

            var lastThirtyDays = await _investecDB.Transactions.Where(t => t.Type.Equals("DEBIT") & t.PostingDate >= DateTime.UtcNow.AddDays(-30)).OrderBy(t => t.PostingDate).ToListAsync();
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

        [HttpGet]
        [Route("WeekOnWeekDays")]
        public async Task<WeekonWeekDays> WeekOnWeekDays()
        {
            List<TransactionDay> lastWeekTransactionsDays = new List<TransactionDay>();
            List<TransactionDay> currentWeekTransactionsDays = new List<TransactionDay>();

            DateTime reference = DateTime.Now;
            Calendar calendar = CultureInfo.CurrentCulture.Calendar;

            IEnumerable<int> daysInMonth = Enumerable.Range(1, calendar.GetDaysInMonth(reference.Year, reference.Month));

            List<Tuple<DateTime, DateTime>> weeks = daysInMonth.Select(day => new DateTime(reference.Year, reference.Month, day))
                .GroupBy(d => calendar.GetWeekOfYear(d, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday))
                .Select(g => new Tuple<DateTime, DateTime>(g.First(), g.Last()))
                .ToList();

            var lastWeekDateRange = weeks.First(w => reference.AddDays(-7) > w.Item1 && reference.AddDays(-7) < w.Item2);
            var currentWeekDateRange = weeks.First(w => reference > w.Item1 && reference < w.Item2);

            for (int i = 0; i < 7; i++)
            {
                lastWeekTransactionsDays.Add(new TransactionDay
                {
                    Date = lastWeekDateRange.Item1.AddDays(i)
                });
                currentWeekTransactionsDays.Add(new TransactionDay
                {
                    Date = currentWeekDateRange.Item1.AddDays(i)
                });
            }

            var lastWeekTransactions = await _investecDB.Transactions.Where(t => t.Type.Equals("DEBIT") & t.PostingDate >= lastWeekDateRange.Item1 & t.PostingDate <= lastWeekDateRange.Item2).OrderBy(t => t.PostingDate).ToListAsync();
            var lastWeekTransactionsGrouped = lastWeekTransactions.GroupBy(lwtd => lwtd.PostingDate.DayOfWeek);
            foreach (var transactionDay in lastWeekTransactionsGrouped)
            {
                lastWeekTransactionsDays.First(lwtd => lwtd.Date.DayOfWeek.Equals(transactionDay.First().PostingDate.DayOfWeek)).Amount = (double)transactionDay.Sum(td => td.Amount);
            }

            var currentWeekTransactions = await _investecDB.Transactions.Where(t => t.Type.Equals("DEBIT") & t.PostingDate >= currentWeekDateRange.Item1 & t.PostingDate <= currentWeekDateRange.Item2).OrderBy(t => t.PostingDate).ToListAsync();
            var currentWeekTransactionsGrouped = currentWeekTransactions.GroupBy(lwtd => lwtd.PostingDate.DayOfWeek);
            foreach (var transactionDay in currentWeekTransactionsGrouped)
            {
                currentWeekTransactionsDays.First(lwtd => lwtd.Date.DayOfWeek.Equals(transactionDay.First().PostingDate.DayOfWeek)).Amount = (double)transactionDay.Sum(td => td.Amount);
            }

            return new WeekonWeekDays
            {
                LastWeek = lastWeekTransactionsDays,
                CurrentWeek = currentWeekTransactionsDays
            };
        }
    }
}