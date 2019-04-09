using System.Collections.Generic;
using System.Linq;

using ManufacturingAPI.Models;
using ManufacturingAPI.Services;

using Xunit;

namespace ManufacturingAPI.UnitTests
{
    public class ProductCheckerTests
    {
        private readonly IProductChecker productChecker;

        public ProductCheckerTests()
        {
            this.productChecker = new DefaultProductChecker();
        }

        [Fact]
        public void ValidProductTypesShouldReturnTrue()
        {
            // Arrange
            var productTypes = new List<string> { "photoBook", "calendar", "canvas", "cards", "mug" };
            var products = productTypes.Select(x => new Product { ProductType = x, Quantity = 1 });

            // Act
            var valid = this.productChecker.IsValidProductList(products, out var error);

            // Assert
            Assert.True(valid, error);
        }

        [Fact]
        public void InvalidProductTypesShouldReturnFalse()
        {
            // Arrange
            var productTypes = new List<string> { "PhotoBook", "photobook", "CALENDAR", "mugs" };
            var products = productTypes.Select(x => new Product { ProductType = x, Quantity = 1 });
            
            // Act
            var valid = this.productChecker.IsValidProductList(products, out _);

            // Assert
            Assert.False(valid);
        }
        
        [Fact]
        public void WrongQuantitiesShouldBeInvalid()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { ProductType = "photoBook", Quantity = 0 },
                new Product { ProductType = "calendar", Quantity = -1 },
            };
            
            // Act / assert
            products.ForEach(p => Assert.False(this.productChecker.IsValidProductList(new[] { p }, out _)));
        }

        [Fact]
        public void PackagingExampleOne()
        {
            // Arrange
            var products = new[]
            {
                new Product { ProductType = "photoBook", Quantity = 1 },
                new Product { ProductType = "calendar", Quantity = 2 },
                new Product { ProductType = "mug", Quantity = 1 },
            };

            // Act
            var width = this.productChecker.CalculateRequiredWidth(products);

            // Assert
            Assert.Equal(19.0 + 10.0 * 2 + 94.0, width);
        }

        [Fact]
        public void PackagingExampleTwo()
        {
            // Arrange
            var products = new[]
            {
                new Product { ProductType = "photoBook", Quantity = 1 },
                new Product { ProductType = "calendar", Quantity = 2 },
                new Product { ProductType = "mug", Quantity = 4 },
            };

            // Act
            var width = this.productChecker.CalculateRequiredWidth(products);

            // Assert
            Assert.Equal(19.0 + 10.0 * 2 + 94.0, width);
        }

        [Fact]
        public void RepeatedProductTypesShouldNotAffectOutcome()
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
            var width = this.productChecker.CalculateRequiredWidth(products);

            // Assert
            Assert.Equal(19.0 + 10.0 * 2 + 94.0 * 2, width);
        }
    }
}
