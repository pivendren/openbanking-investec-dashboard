using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(Investec.Dashboard.Function.Startup))]

namespace Investec.Dashboard.Function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
            string SqlConnection = Environment.GetEnvironmentVariable("SqlConnectionString");
            builder.Services.AddDbContext<InvestecDBContext>(options => options.UseSqlServer(SqlConnection));
        }
    }
}