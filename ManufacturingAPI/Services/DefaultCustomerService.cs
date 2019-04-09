using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

using AutoMapper;

using ManufacturingAPI.Models;

namespace ManufacturingAPI.Services
{
    public class DefaultCustomerService : ICustomerService
    {
        private readonly IDynamoDBContext context;

        private readonly IMapper mapper;

        public DefaultCustomerService(IDynamoDBContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            var results = await this.context.ScanAsync<CustomerEntity>(new[]
            {
                new ScanCondition(nameof(CustomerEntity.CustomerId), ScanOperator.BeginsWith, CustomerEntity.Prefix),
            }).GetRemainingAsync();

            return results.Select(x => this.mapper.Map<Customer>(x));
        }

        public async Task<Customer> GetCustomerByIdAsync(string customerId)
        {
            var results = await this.context.QueryAsync<CustomerEntity>(CustomerEntity.Prefix + customerId).GetRemainingAsync();
            return this.mapper.Map<Customer>(results.FirstOrDefault());
        }
    }
}
