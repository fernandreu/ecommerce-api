using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceAPI.ApplicationCore.Entities;
using ECommerceAPI.ApplicationCore.Interfaces;

namespace ECommerceAPI.Infrastructure.Services
{
    public class ProductChecker : IProductChecker
    {
        private readonly IProductTypeRepository productTypeRepository;

        public ProductChecker(IProductTypeRepository productTypeRepository)
        {
            this.productTypeRepository = productTypeRepository;
        }

        public async Task<Tuple<bool, string>> IsValidProductListAsync(IEnumerable<Product> products)
        {
            var productList = products?.ToList();

            if (productList == null || productList.Count == 0)
            {
                return Tuple.Create(false, "The list of products cannot be null or empty");
            }

            foreach (var product in productList)
            {
                if (product.ProductType == null)
                {
                    return Tuple.Create(false, "The ProductType was not specified");
                }

                var productType = await this.productTypeRepository.GetByNameAsync(product.ProductType);
                if (productType == null)
                {
                    return Tuple.Create(false, $"Invalid product type: {product.ProductType}");
                }

                if (product.Quantity < 1)
                {
                    return Tuple.Create(false, "The Quantity must be positive");
                }
            }

            return Tuple.Create<bool, string>(true, null);
        }

        public async Task<double> CalculateRequiredWidthAsync(IEnumerable<Product> products)
        {
            var productList = products.ToList();

            var (valid, error) = await this.IsValidProductListAsync(productList);
            if (!valid)
            {
                throw new ArgumentException(error);
            }

            // Combine all products of the same type (just in case they are not all specified at once)
            var types = new Dictionary<string, Tuple<ProductType, int>>();
            foreach (var product in productList)
            {
                if (!types.ContainsKey(product.ProductType))
                {
                    var productType = await this.productTypeRepository.GetByNameAsync(product.ProductType);
                    if (productType == null)
                    {
                        // Unlikely, since the IsValidProductListAsync check was passed
                        throw new ArgumentException($"The productType {product.ProductType} was not found");
                    }

                    types.Add(product.ProductType, Tuple.Create(productType, product.Quantity));
                }
                else
                {
                    var (productType, count) = types[product.ProductType];
                    types[product.ProductType] = Tuple.Create(productType, count + product.Quantity);
                }
            }

            // Calculate the actual width
            var result = 0.0;
            foreach (var (productType, count) in types.Values)
            {
                result += productType.Width * count;
            }

            return result;
        }
    }
}
