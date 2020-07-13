using Investec.Dashboard.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Investec.Dashboard.Function
{
    public class InvestecDBContext : DbContext
    {
        public InvestecDBContext(DbContextOptions<InvestecDBContext> options) : base(options)
        {
        }

        public DbSet<TransactionEntity> Transactions { get; set; }
    }
}