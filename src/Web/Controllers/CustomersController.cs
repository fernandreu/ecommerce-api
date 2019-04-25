using System.Linq;
using System.Threading.Tasks;

using ECommerceAPI.Web.Helpers;
using ECommerceAPI.Web.Interfaces;
using ECommerceAPI.Web.Resources;

using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Web.Controllers
{
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService customerService;

        public CustomersController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }
        
        [HttpGet(Name = nameof(GetAllCustomers))]
        [ProducesResponseType(200)]
        public async Task<ActionResult<Collection<CustomerResource>>> GetAllCustomers()
        {
            var customers = await this.customerService.GetAllCustomersAsync();
            return new Collection<CustomerResource>
            {
                Self = Link.ToCollection(nameof(this.GetAllCustomers)),
                Value = customers.ToArray(),
            };
        }

        [HttpGet("{customerId}", Name = nameof(GetCustomerById))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CustomerResource>> GetCustomerById(string customerId)
        {
            var customer = await this.customerService.GetCustomerByIdAsync(customerId);
            if (customer == null)
            {
                return this.NotFound(new ApiError(404, "The customerId was not found"));
            }

            return customer;
        }
    }
}
