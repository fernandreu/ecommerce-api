using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using ECommerceAPI.ApplicationCore.Interfaces;
using ECommerceAPI.Web.Interfaces;
using ECommerceAPI.Web.Resources;

namespace ECommerceAPI.Web.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository customerRepository;

        private readonly IConfigurationProvider mappingConfiguration;

        public CustomerService(ICustomerRepository customerRepository, IConfigurationProvider mappingConfiguration)
        {
            this.customerRepository = customerRepository;
            this.mappingConfiguration = mappingConfiguration;
        }

        public async Task<IEnumerable<CustomerResource>> GetAllCustomersAsync()
        {
            var results = await this.customerRepository.GetAllAsync();
            return results.AsQueryable().ProjectTo<CustomerResource>(this.mappingConfiguration);
        }

        public async Task<CustomerResource> GetCustomerByIdAsync(string customerId)
        {
            var result = await this.customerRepository.GetByIdAsync(customerId);
            var mapper = this.mappingConfiguration.CreateMapper();
            return mapper.Map<CustomerResource>(result);
        }
    }
}
