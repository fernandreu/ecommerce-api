using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

using AutoMapper;

using ManufacturingAPI.Extensions;
using ManufacturingAPI.Models;

namespace ManufacturingAPI.Services
{
    public class DefaultOrderService : IOrderService
    {
        private readonly IDynamoDBContext context;

        private readonly IMapper mapper;

        private readonly ICustomerService customerService;

        private readonly IProductChecker productChecker;

        public DefaultOrderService(IDynamoDBContext context, IMapper mapper, ICustomerService customerService, IProductChecker productChecker)
        {
            this.context = context;
            this.mapper = mapper;
            this.customerService = customerService;
            this.productChecker = productChecker;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync(string customerId)
        {
            if (await this.customerService.GetCustomerByIdAsync(customerId) == null)
            {
                return null;
            }

            var results = await this.context.ScanAsync<OrderEntity>(new[]
            {
                new ScanCondition(nameof(OrderEntity.OrderId), ScanOperator.BeginsWith, OrderEntity.Prefix),
                new ScanCondition(nameof(OrderEntity.CustomerId), ScanOperator.Equal, CustomerEntity.Prefix + customerId),
            }).GetRemainingAsync();
            
            return results.Select(x => this.mapper.MapOrderFull(x, this.productChecker));
        }

        public async Task<Order> GetOrderByIdAsync(string customerId, string orderId)
        {
            if (await this.customerService.GetCustomerByIdAsync(customerId) == null)
            {
                return null;
            }

            var results = await this.context.ScanAsync<OrderEntity>(new[]
            {
                new ScanCondition(nameof(OrderEntity.OrderId), ScanOperator.Equal, OrderEntity.Prefix + orderId),
                new ScanCondition(nameof(OrderEntity.CustomerId), ScanOperator.Equal, CustomerEntity.Prefix + customerId),
            }).GetRemainingAsync();

            return this.mapper.MapOrderFull(results.FirstOrDefault(), this.productChecker);
        }
        
        public async Task<Order> SaveOrderAsync(string customerId, string orderId, Order order)
        {
            if (await this.customerService.GetCustomerByIdAsync(customerId) == null)
            {
                throw new ArgumentException("The clientId specified is invalid");
            }

            if (!this.productChecker.IsValidProductList(order.Products, out var error))
            {
                throw new ArgumentException(error);
            }

            var orderEntity = new OrderEntity
            {
                OrderId = OrderEntity.Prefix + orderId,
                CustomerId = CustomerEntity.Prefix + customerId,
                Products = order.Products.ToList(),
                OrderDate = order.OrderDate,
            };

            await this.context.SaveAsync(orderEntity);
            
            return this.mapper.MapOrderFull(orderEntity, this.productChecker);
        }
    }
}
