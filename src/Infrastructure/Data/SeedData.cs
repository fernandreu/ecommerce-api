using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using ECommerceAPI.ApplicationCore.Entities;
using ECommerceAPI.Infrastructure.Entries;
using ECommerceAPI.Infrastructure.Extensions;

using Microsoft.Extensions.DependencyInjection;
using ProductTypeEntry = ECommerceAPI.Infrastructure.Entries.ProductTypeEntry;

namespace ECommerceAPI.Infrastructure.Data
{
    /// <summary>
    /// Handles the creation of dummy CustomerResource / OrderResource entries at the start of the application
    /// </summary>
    public static class SeedData
    {
        public static readonly ProductType[] Products = {
            new ProductType
            {
                Name = "photoBook", 
                Width = 19.0, 
                Height = 20.0, 
                Depth = 5.0, 
                Price = 5.0, 
                Weight = 12.0
            },
            new ProductType
            {
                Name = "calendar",
                Width = 10.0, 
                Height = 35.0, 
                Depth = 12.0, 
                Price = 3.5, 
                Weight = 5.6
            },
            new ProductType
            {
                Name = "canvas", 
                Width = 16.0, 
                Height = 35.0, 
                Depth = 12.0, 
                Price = 3.5, 
                Weight = 3.2
            },
            new ProductType
            {
                Name = "cards", 
                Width = 4.7, 
                Height = 35.0, 
                Depth = 12.0, 
                Price = 3.5, 
                Weight = 7.6
            },
            new ProductType
            {
                Name = "mug", 
                Width = 94.0, 
                Height = 100.0, 
                Depth = 94.0, 
                Price = 7.2, 
                Weight = 2.5
            },
        };

        public static async Task InitializeAsync(IServiceProvider services)
        {
            var client = services.GetRequiredService<IAmazonDynamoDB>();
            await client.CreateMainTableAsync();

            var context = services.GetRequiredService<IDynamoDBContext>();
            var mapper = services.GetRequiredService<IMapper>();
            await AddTestDataAsync(context, mapper);
        }

        public static async Task AddTestDataAsync(IDynamoDBContext context, IMapper mapper, bool force = true)
        {
            // Check if database already contains data, in which case we don't add anything
            var customers = await context.ScanAsync<CustomerEntry>(new List<ScanCondition>()).GetRemainingAsync();
            if (customers.Any() && !force)
            {
                return;
            }

            // Add ProductTypes
            var id = 0;
            var products = new List<ProductTypeEntry>();
            foreach (var p in Products)
            {
                var product = mapper.Map<ProductTypeEntry>(p);
                product.Id = ProductTypeEntry.Prefix + ++id;
                products.Add(product);
            }
            await Task.WhenAll(products.Select(p => context.SaveAsync(p)));

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
