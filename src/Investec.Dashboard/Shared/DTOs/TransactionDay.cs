using System;

namespace Investec.Dashboard.Shared.DTOs
{
    public class TransactionDay
    {
        public DateTime Date { get; set; }
        public int Day { get { return Date.Day; } }
        public string Type { get; set; }
        public double Amount { get; set; }
    }
}