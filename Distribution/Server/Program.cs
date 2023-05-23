using Distribution.Client;
using Distribution.Server;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Distribution
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                try
                {
                    var context = services.GetRequiredService<Unicontext>();


                    if (context.Database.IsSqlServer())
                    {
                        context.Database.Migrate();
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating or seeding the database.");
                    throw;
                }
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)

                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStaticWebAssets();
                    webBuilder.UseStartup<Startup>();
                });
    }
}