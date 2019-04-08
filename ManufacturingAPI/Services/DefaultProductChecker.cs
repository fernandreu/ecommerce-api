using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ManufacturingAPI.Models;

namespace ManufacturingAPI.Services
{
    public class DefaultProductChecker : IProductChecker
    {
        private readonly Dictionary<string, ProductDetails> validProducts = new Dictionary<string, ProductDetails>
        {
            ["photoBook"] = new ProductDetails(19.0),
            ["calendar"] = new ProductDetails(10.0),
            ["canvas"] = new ProductDetails(16.0),
            ["cards"] = new ProductDetails(4.7),
            ["mug"] = new ProductDetails(94.0, 4),
        }; 

        public bool IsValidProduct(string productType)
        {
            return this.validProducts.ContainsKey(productType);
        }

        public double CalculateRequiredWidth(IEnumerable<Product> products)
        {
            // Combine all products of the same type (just in case they are not all specified at once)
            var dict = new Dictionary<ProductDetails, int>();
            foreach (var product in products)
            {
                if (!this.validProducts.TryGetValue(product.ProductType, out var details))
                {
                    throw new ArgumentException($"Invalid product type: {product.ProductType}");
                }
                
                var count = dict.GetValueOrDefault(details, 0);
                dict[details] = count + product.Quantity;
            }

            // Calculate the actual width
            var result = 0.0;
            foreach (var (details, count) in dict)
            {
                result += ((count - 1) / details.MaxStack + 1) * details.RequiredWidth;
            }

            return result;
        }

        private class ProductDetails
        {
            public ProductDetails(double requiredWidth, int maxStack = 1)
            {
                this.RequiredWidth = requiredWidth;
                this.MaxStack = maxStack;
            }

            public double RequiredWidth { get; }

            public int MaxStack { get; }
        }

    }
}
