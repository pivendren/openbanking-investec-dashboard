using System;

namespace Investec.Dashboard.Data.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public string AccountId { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string CardNumber { get; set; }
        public DateTime PostingDate { get; set; }
        public DateTime ValueDate { get; set; }
        public DateTime ActionDate { get; set; }
        public float Amount { get; set; }
    }
}