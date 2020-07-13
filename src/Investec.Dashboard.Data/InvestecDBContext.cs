using Investec.Dashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Investec.Dashboard.Data
{
    public class InvestecDBContext : DbContext
    {
        public InvestecDBContext(DbContextOptions<InvestecDBContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=InvestecDb;Trusted_Connection=True;MultipleActiveResultSets=true");
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Transaction> Transactions { get; set; }
    }
}