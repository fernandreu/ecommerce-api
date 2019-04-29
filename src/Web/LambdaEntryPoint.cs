using System;

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

using ECommerceAPI.Infrastructure.Data;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ECommerceAPI.Web
{
    /// <summary>
    /// This class extends from APIGatewayProxyFunction which contains the method FunctionHandlerAsync which is the 
    /// actual Lambda function entry point. The Lambda handler field should be set to
    /// Web::ECommerceAPI.Web.LambdaEntryPoint::FunctionHandlerAsync
    /// </summary>
    public class LambdaEntryPoint :

        // When using an ELB's Application Load Balancer as the event source change 
        // the base class to Amazon.Lambda.AspNetCoreServer.ApplicationLoadBalancerFunction
        Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction
    {
        /// <summary>
        /// The builder has configuration, logging and Amazon API Gateway already configured. The startup class
        /// needs to be configured in this method using the UseStartup<>() method.
        /// </summary>
        /// <param name="builder"></param>
        protected override void Init(IWebHostBuilder builder)
        {
            Console.WriteLine("Initiating lambda");
            builder.UseStartup<Startup>();
        }

        protected override void PostCreateWebHost(IWebHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                
                var logger = services.GetRequiredService<ILogger<LambdaEntryPoint>>();
                logger.LogError("Prior to getting client");
                
                var client = services.GetRequiredService<IAmazonDynamoDB>();
                logger.LogError("After getting client");
                logger.LogError($"Client region: {client.Config.RegionEndpoint.DisplayName}");
                throw new Exception($"client: {client.Config.RegionEndpoint.DisplayName}");

                try
                {
                    SeedData.AddTestDataAsync(services.GetRequiredService<IDynamoDBContext>()).Wait();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the database.");
                }
            }
        }
    }
}
