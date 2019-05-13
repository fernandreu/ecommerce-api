using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using ECommerceAPI.ApplicationCore.Entities;
using ECommerceAPI.ApplicationCore.Interfaces;
using ECommerceAPI.Web.Interfaces;
using ECommerceAPI.Web.Resources;

namespace ECommerceAPI.Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly IConfigurationProvider mappingConfiguration;

        private readonly ICustomerRepository customerRepository;

        private readonly IOrderRepository orderRepository;

        private readonly IProductChecker productChecker;

        public OrderService(IConfigurationProvider mappingConfiguration, ICustomerRepository customerRepository, IOrderRepository orderRepository, IProductChecker productChecker)
        {
            this.mappingConfiguration = mappingConfiguration;
            this.customerRepository = customerRepository;
            this.orderRepository = orderRepository;
            this.productChecker = productChecker;
        }

        public async Task<IEnumerable<OrderResource>> GetAllOrdersAsync(string customerId)
        {
            if (await this.customerRepository.GetByIdAsync(customerId) == null)
            {
                return null;
            }

            var results = await this.orderRepository.GetAllAsync();

            // TODO: This is highly inefficient (all database results are being returned)
            results = results.Where(x => x.CustomerId == customerId);

            return results.AsQueryable().ProjectTo<OrderResource>(this.mappingConfiguration);
        }

        public async Task<OrderResource> GetOrderByIdAsync(string customerId, string orderId)
        {
            var customer = await this.customerRepository.GetByIdAsync(customerId);
            var order = await this.orderRepository.GetByIdAsync(orderId);

            if (customer == null || order == null || order.CustomerId != customerId)
            {
                return null;
            }

            var mapper = this.mappingConfiguration.CreateMapper();
            return mapper.Map<OrderResource>(order);
        }
        
        public async Task<OrderResource> SaveOrderAsync(string customerId, string orderId, OrderResource orderResource)
        {
            if (await this.customerRepository.GetByIdAsync(customerId) == null)
            {
                throw new ArgumentException("The customerId specified is invalid");
            }

            var (valid, error) = await this.productChecker.IsValidProductListAsync(orderResource.Products);
            if (!valid)
            {
                throw new ArgumentException(error);
            }

            var order = new Order
            {
                Id = orderId,
                CustomerId = customerId,
                Products = orderResource.Products.ToList(),
                OrderDate = orderResource.OrderDate,
            };

            await this.orderRepository.PutAsync(order);
            
            var mapper = this.mappingConfiguration.CreateMapper();
            return mapper.Map<OrderResource>(order);
        }
    }
}
