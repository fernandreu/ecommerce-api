using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.DynamoDBv2;

using ManufacturingAPI.Extensions;
using ManufacturingAPI.Models;

namespace ManufacturingAPI
{
    using Amazon.DynamoDBv2.DataModel;

    using Microsoft.Extensions.DependencyInjection;

    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var client = services.GetRequiredService<IAmazonDynamoDB>();
            await client.CreateMainTableAsync();

            var context = services.GetRequiredService<IDynamoDBContext>();
            await AddTestDataAsync(context);
        }

        public static async Task AddTestDataAsync(IDynamoDBContext context)
        {
            // Check if database already contains data, in which case we don't add anything
            var customers = await context.ScanAsync<CustomerEntity>(new List<ScanCondition>()).GetRemainingAsync();
            if (customers.Any())
            {
                return;
            }
            
            var testCustomer = new CustomerEntity
            {
                CustomerId = CustomerEntity.Prefix + "TEST",
                ContactName = "John Doe",
                Address = "Amsterdam",
            };

            var normalCustomer = new CustomerEntity
            {
                CustomerId = CustomerEntity.Prefix + "1",
                ContactName = "Fernando Andreu",
                Address = "Glasgow",
            };

            await context.SaveAsync(testCustomer);
            await context.SaveAsync(normalCustomer);

            await context.SaveAsync(new OrderEntity
            {
                OrderId = OrderEntity.Prefix + "1",
                CustomerId = testCustomer.CustomerId,
                OrderDate = "2019/04/07",
                Products = new List<Product>
                {
                    new Product { ProductType = "photoBook", Quantity = 1 },
                    new Product { ProductType = "mug", Quantity = 3 },
                }
            });

            await context.SaveAsync(new OrderEntity
            {
                OrderId = OrderEntity.Prefix + "2",
                CustomerId = testCustomer.CustomerId,
                OrderDate = "2019/04/08",
                Products = new List<Product>
                {
                    new Product { ProductType = "calendar", Quantity = 1 },
                    new Product { ProductType = "canvas", Quantity = 2 },
                }
            });

            await context.SaveAsync(new OrderEntity
            {
                OrderId = OrderEntity.Prefix + "3",
                CustomerId = normalCustomer.CustomerId,
                OrderDate = "2019/04/08",
                Products = new List<Product>
                {
                    new Product { ProductType = "mug", Quantity = 1 },
                    new Product { ProductType = "cards", Quantity = 4 },
                }
            });
        }
    }
}
