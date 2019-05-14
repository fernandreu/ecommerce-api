using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

using AutoMapper;

using ECommerceAPI.ApplicationCore.Interfaces;
using ECommerceAPI.Infrastructure.Data;
using ECommerceAPI.Infrastructure.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerceAPI.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDynamoDBContext, DynamoDBContext>();
            
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
            services.AddSingleton<IProductChecker, ProductChecker>();
            
            services.AddDefaultAWSOptions(configuration.GetAWSOptions());

            var dynamoDbConfig = configuration.GetSection("DynamoDb");
            var runLocalDynamoDb = dynamoDbConfig.GetValue<bool>("LocalMode");
            if (runLocalDynamoDb)
            {
                services.AddSingleton<IAmazonDynamoDB>(sp =>
                {
                    var clientConfig = new AmazonDynamoDBConfig { ServiceURL = dynamoDbConfig.GetValue<string>("LocalServiceUrl") };
                    return new AmazonDynamoDBClient(clientConfig);
                });
            }
            else
            {
                services.AddAWSService<IAmazonDynamoDB>();
            }

            return services;
        }

        public static void AddInfrastructureProfile(this IMapperConfigurationExpression options)
        {
            options.AddProfile<EntryMappingProfile>();
        }
    }
}
