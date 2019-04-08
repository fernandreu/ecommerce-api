using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ManufacturingAPI
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The Main function can be used to run the ASP.NET Core application locally using the Kestrel webserver.
    /// </summary>
    public class LocalEntryPoint
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHost(args);
            InitializeDatabase(host);
            host.Run();
        }

        public static IWebHost CreateWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

        public static void InitializeDatabase(IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    SeedData.InitializeAsync(services).Wait();
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
