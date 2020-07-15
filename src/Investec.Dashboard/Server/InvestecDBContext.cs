using Investec.Dashboard.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Investec.Dashboard.Server
{
    public class InvestecDBContext : DbContext
    {
        public InvestecDBContext(DbContextOptions<InvestecDBContext> options) : base(options)
        {
        }

        public DbSet<TransactionEntity> Transactions { get; set; }
        public DbSet<Budget> Budget { get; set; }
    }
}