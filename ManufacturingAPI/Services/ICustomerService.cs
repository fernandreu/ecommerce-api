using System.Collections.Generic;
using System.Threading.Tasks;

using ManufacturingAPI.Models;

namespace ManufacturingAPI.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();

        Task<Customer> GetCustomerByIdAsync(string customerId);
    }
}
