using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceAPI.ApplicationCore.Entities;
using ECommerceAPI.ApplicationCore.Interfaces;
using ECommerceAPI.Infrastructure.Services;
using Xunit;

namespace ECommerceAPI.UnitTests.Fixtures
{
    public class ProductCheckerTests
    {
        private readonly IProductChecker productChecker;

        public ProductCheckerTests()
        {
            this.productChecker = new ProductChecker(new MockProductTypeRepository());
        }

        [Fact]
        public async Task ValidProductTypesShouldReturnTrue()
        {
            // Arrange
            var productTypes = new List<string> { "photoBook", "calendar", "canvas", "cards", "mug" };
            var products = productTypes.Select(x => new Product { ProductType = x, Quantity = 1 });

            // Act
            var (valid, error) = await this.productChecker.IsValidProductListAsync(products);

            // Assert
            Assert.True(valid, error);
        }

        [Fact]
        public async Task InvalidProductTypesShouldReturnFalse()
        {
            // Arrange
            var productTypes = new List<string> { "PhotoBook", "photobook", "CALENDAR", "mugs" };
            var products = productTypes.Select(x => new Product { ProductType = x, Quantity = 1 });
            
            // Act
            var (valid, _) = await this.productChecker.IsValidProductListAsync(products);

            // Assert
            Assert.False(valid);
        }
        
        [Fact]
        public async Task WrongQuantitiesShouldBeInvalid()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { ProductType = "photoBook", Quantity = 0 },
                new Product { ProductType = "calendar", Quantity = -1 },
            };
            
            // Act / assert
            foreach (var p in products)
            {
                var (valid, _) = await this.productChecker.IsValidProductListAsync(new[] {p});
                Assert.False(valid);
            }
        }

        [Fact]
        public async Task PackagingExampleOne()
        {
            // Arrange
            var products = new[]
            {
                new Product { ProductType = "photoBook", Quantity = 1 },
                new Product { ProductType = "calendar", Quantity = 2 },
                new Product { ProductType = "mug", Quantity = 1 },
            };

            // Act
            var width = await this.productChecker.CalculateRequiredWidthAsync(products);

            // Assert
            Assert.Equal(19.0 + 10.0 * 2 + 94.0, width);
        }

        [Fact]
        public async Task PackagingExampleTwo()
        {
            // Arrange
            var products = new[]
            {
                new Product { ProductType = "photoBook", Quantity = 1 },
                new Product { ProductType = "calendar", Quantity = 2 },
                new Product { ProductType = "mug", Quantity = 4 },
            };

            // Act
            var width = await this.productChecker.CalculateRequiredWidthAsync(products);

            // Assert
            Assert.Equal(19.0 + 10.0 * 2 + 94.0 * 4, width);
        }

        [Fact]
        public async Task RepeatedProductTypesShouldNotAffectOutcome()
        {
            // Arrange
            var products = new[]
            {
                new Product { ProductType = "mug", Quantity = 1 },
                new Product { ProductType = "photoBook", Quantity = 1 },
                new Product { ProductType = "calendar", Quantity = 1 },
                new Product { ProductType = "mug", Quantity = 1 },
                new Product { ProductType = "calendar", Quantity = 1 },
                new Product { ProductType = "mug", Quantity = 3 },
                new Product { ProductType = "mug", Quantity = 1 },
            };

            // Act
            var width = await this.productChecker.CalculateRequiredWidthAsync(products);

            // Assert
            Assert.Equal(19.0 + 10.0 * 2 + 94.0 * 6, width);
        }

        private class MockProductTypeRepository : IProductTypeRepository
        {
            private readonly Dictionary<string, double> productWidths = new Dictionary<string, double>
            {
                { "photoBook", 19.0 },
                { "calendar", 10.0 },
                { "canvas", 16.0 },
                { "cards", 4.7 },
                { "mug", 94.0 },
            };

            public Task<ProductType> GetByIdAsync(string id)
            {
                throw new NotImplementedException();
            }

            public Task<IEnumerable<ProductType>> GetAllAsync()
            {
                throw new NotImplementedException();
            }

            public Task<ProductType> PutAsync(ProductType entity)
            {
                throw new NotImplementedException();
            }

            public Task<ProductType> PostAsync(ProductType entity)
            {
                throw new NotImplementedException();
            }

            public Task<ProductType> GetByNameAsync(string name)
            {
                if (!this.productWidths.ContainsKey(name))
                {
                    return Task.FromResult<ProductType>(null);
                }

                return Task.FromResult(new ProductType
                {
                    Name = name,
                    Width = this.productWidths[name],
                });
            }
        }
    }
}
