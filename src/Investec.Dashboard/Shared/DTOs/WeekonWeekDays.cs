using System.Collections.Generic;

namespace Investec.Dashboard.Shared.DTOs
{
    public class WeekonWeekDays
    {
        public IEnumerable<TransactionDay> LastWeek { get; set; }
        public IEnumerable<TransactionDay> CurrentWeek { get; set; }
    }
}