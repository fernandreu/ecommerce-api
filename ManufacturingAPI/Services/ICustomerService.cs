using System.Collections.Generic;
using System.Threading.Tasks;

using ManufacturingAPI.Models;

namespace ManufacturingAPI.Services
{
    /// <summary>
    /// Contains several methods to perform CRUD operations with Customer entries
    /// </summary>
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();

        Task<Customer> GetCustomerByIdAsync(string customerId);
    }
}
