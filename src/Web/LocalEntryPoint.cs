using System;
using System.Threading.Tasks;
using ECommerceAPI.Infrastructure.Data;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ECommerceAPI.Web
{
    /// <summary>
    /// The Main function can be used to run the ASP.NET Core application locally using the Kestrel webserver.
    /// </summary>
    public class LocalEntryPoint
    {
        public static async Task Main(string[] args)
        {
            var host = CreateWebHost(args);
            await InitializeDatabaseAsync(host);
            host.Run();
        }

        public static IWebHost CreateWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

        public static async Task InitializeDatabaseAsync(IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    await SeedData.InitializeAsync(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<LocalEntryPoint>>();
                    logger.LogError(ex, "An error occurred seeding the database.");
                    
                    // TODO: This is temporary for debugging purposes
                    throw;
                }
            }
        }
    }
}
