using System.Linq;
using System.Threading.Tasks;

using Amazon.DynamoDBv2.DataModel;

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

        public async Task<Customer> GetCustomerByIdAsync(string customerId)
        {
            var results = await this.context.QueryAsync<CustomerEntity>(CustomerEntity.Prefix + customerId).GetRemainingAsync();
            return this.mapper.Map<Customer>(results.FirstOrDefault());
        }
    }
}
