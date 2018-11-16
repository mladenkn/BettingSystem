using System.Threading.Tasks;
using BetingSystem.DevelopmentUtilities;
using BetingSystem.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BetingSystem.RestApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<DbContext>();
                var bonusesAccessor = scope.ServiceProvider.GetService<ITicketBonusesAccessor>();
                await DbSeeder.Seed(db, bonusesAccessor.Value);
            }
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
