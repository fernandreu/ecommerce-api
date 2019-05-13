using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

using ECommerceAPI.ApplicationCore.Entities;
using ECommerceAPI.Infrastructure.Entries;
using ECommerceAPI.Infrastructure.Extensions;

using Microsoft.Extensions.DependencyInjection;

namespace ECommerceAPI.Infrastructure.Data
{
    /// <summary>
    /// Handles the creation of dummy CustomerResource / OrderResource entries at the start of the application
    /// </summary>
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var client = services.GetRequiredService<IAmazonDynamoDB>();
            await client.CreateMainTableAsync();

            var context = services.GetRequiredService<IDynamoDBContext>();
            await AddTestDataAsync(context);
        }

        public static async Task AddTestDataAsync(IDynamoDBContext context, bool force = true)
        {
            // Check if database already contains data, in which case we don't add anything
            var customers = await context.ScanAsync<CustomerEntry>(new List<ScanCondition>()).GetRemainingAsync();
            if (customers.Any() && !force)
            {
                return;
            }

            // Add ProductTypes
            var id = 0;
            await context.SaveAsync(new ProductTypeEntry
            {
                Id = ProductTypeEntry.Prefix + ++id,
                Name = "photoBook",
                Width = 19.0,
            });
            await context.SaveAsync(new ProductTypeEntry
            {
                Id = ProductTypeEntry.Prefix + ++id,
                Name = "calendar",
                Width = 10.0,
            });
            await context.SaveAsync(new ProductTypeEntry
            {
                Id = ProductTypeEntry.Prefix + ++id,
                Name = "canvas",
                Width = 16.0,
            });
            await context.SaveAsync(new ProductTypeEntry
            {
                Id = ProductTypeEntry.Prefix + ++id,
                Name = "cards",
                Width = 4.7,
            });
            await context.SaveAsync(new ProductTypeEntry
            {
                Id = ProductTypeEntry.Prefix + ++id,
                Name = "mug",
                Width = 94.0,
            });

            var testCustomer = new CustomerEntry
            {
                Id = CustomerEntry.Prefix + "TEST",
                ContactName = "John Doe",
                Address = "Amsterdam",
            };

            var normalCustomer = new CustomerEntry
            {
                Id = CustomerEntry.Prefix + "1",
                ContactName = "Fernando Andreu",
                Address = "Glasgow",
            };

            await context.SaveAsync(testCustomer);
            await context.SaveAsync(normalCustomer);

            await context.SaveAsync(new OrderEntry
            {
                Id = OrderEntry.Prefix + "1",
                CustomerId = testCustomer.Id,
                OrderDate = "2019/04/07",
                Products = new List<Product>
                {
                    new Product { ProductType = "photoBook", Quantity = 1 },
                    new Product { ProductType = "mug", Quantity = 3 },
                }
            });

            await context.SaveAsync(new OrderEntry
            {
                Id = OrderEntry.Prefix + "2",
                CustomerId = testCustomer.Id,
                OrderDate = "2019/04/08",
                Products = new List<Product>
                {
                    new Product { ProductType = "calendar", Quantity = 1 },
                    new Product { ProductType = "canvas", Quantity = 2 },
                }
            });

            await context.SaveAsync(new OrderEntry
            {
                Id = OrderEntry.Prefix + "3",
                CustomerId = normalCustomer.Id,
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
