using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ManufacturingAPI.Models;
using ManufacturingAPI.Services;

using Microsoft.AspNetCore.Mvc;

namespace ManufacturingAPI.Controllers
{
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService customerService;

        public CustomersController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }
        
        [HttpGet("{customerId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Customer>> GetCustomerById(string customerId)
        {
            var customer = await this.customerService.GetCustomerByIdAsync(customerId);
            if (customer == null)
            {
                return this.NotFound();
            }

            return customer;
        }

    }
}
