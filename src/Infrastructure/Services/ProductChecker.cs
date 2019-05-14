using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceAPI.ApplicationCore.Entities;
using ECommerceAPI.ApplicationCore.Interfaces;
using ECommerceAPI.ApplicationCore.Logistics;

namespace ECommerceAPI.Infrastructure.Services
{
    public class ProductChecker : IProductChecker
    {
        private readonly IProductTypeRepository productTypeRepository;

        public ProductChecker(IProductTypeRepository productTypeRepository)
        {
            this.productTypeRepository = productTypeRepository;
        }

        public async Task<(bool valid, string error)> IsValidProductListAsync(IEnumerable<Product> products)
        {
            var productList = products?.ToList();

            if (productList == null || productList.Count == 0)
            {
                return (false, "The list of products cannot be null or empty");
            }

            foreach (var product in productList)
            {
                if (product.ProductType == null)
                {
                    return (false, "The ProductType was not specified");
                }

                var productType = await this.productTypeRepository.GetByNameAsync(product.ProductType);
                if (productType == null)
                {
                    return (false, $"Invalid product type: {product.ProductType}");
                }

                if (product.Quantity < 1)
                {
                    return (false, "The Quantity must be positive");
                }
            }

            return (true, null);
        }

        public async Task<OrderDetails> CalculateOrderDetailsAsync(Order order)
        {
            var (valid, error) = await this.IsValidProductListAsync(order.Products);
            if (!valid)
            {
                throw new ArgumentException(error);
            }
            
            // Combine all products of the same type (just in case they are not all specified at once)
            var types = new Dictionary<string, (ProductType type, int count)>();
            foreach (var product in order.Products)
            {
                if (!types.ContainsKey(product.ProductType))
                {
                    var productType = await this.productTypeRepository.GetByNameAsync(product.ProductType);
                    if (productType == null)
                    {
                        // Unlikely, since the IsValidProductListAsync check was passed
                        throw new ArgumentException($"The productType {product.ProductType} was not found");
                    }

                    types.Add(product.ProductType, (productType, product.Quantity));
                }
                else
                {
                    var tuple = types[product.ProductType];
                    tuple.count += product.Quantity;
                    types[product.ProductType] = tuple;
                }
            }

            // Calculate the actual order details
            var result = new OrderDetails();
            foreach (var (productType, count) in types.Values)
            {
                // TODO: Right now, the algorithm is quite simple, as it just piles up products on top of each other
                result.Width = Math.Max(result.Width, productType.Width);
                result.Depth = Math.Max(result.Depth, productType.Depth);
                result.Height += productType.Height * count;
                result.Weight += productType.Weight * count;
                result.Price += productType.Price * count;
            }

            return result;
        }
    }
}
