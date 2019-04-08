using System.Collections.Generic;

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
            var validProducts = new List<string> { "photoBook", "calendar", "canvas", "cards", "mug" };

            // Act / assert
            validProducts.ForEach(p => Assert.True(this.productChecker.IsValidProduct(p)));
        }

        [Fact]
        public void InvalidProductTypesShouldReturnFalse()
        {
            // Arrange
            var validProducts = new List<string> { "PhotoBook", "photobook", "CALENDAR", "mugs" };

            // Act / assert
            validProducts.ForEach(p => Assert.False(this.productChecker.IsValidProduct(p)));
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
