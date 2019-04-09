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

        public bool IsValidProductList(IEnumerable<Product> products, out string errorMessage)
        {
            var productList = products?.ToList();

            if (productList == null || productList.Count == 0)
            {
                errorMessage = "The list of products cannot be null or empty";
                return false;
            }

            foreach (var product in productList)
            {
                if (product.ProductType == null)
                {
                    errorMessage = "The ProductType was not specified";
                    return false;
                }

                if (!this.validProducts.ContainsKey(product.ProductType))
                {
                    errorMessage = $"Invalid product type: {product.ProductType}";
                    return false;
                }

                if (product.Quantity < 1)
                {
                    errorMessage = "The Quantity must be positive";
                    return false;
                }
            }

            errorMessage = null;
            return true;
        }

        public double CalculateRequiredWidth(IEnumerable<Product> products)
        {
            var productList = products.ToList();

            if (!this.IsValidProductList(productList, out var error))
            {
                throw new ArgumentException(error);
            }

            // Combine all products of the same type (just in case they are not all specified at once)
            var dict = new Dictionary<ProductDetails, int>();
            foreach (var product in productList)
            {
                var details = this.validProducts[product.ProductType];
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
