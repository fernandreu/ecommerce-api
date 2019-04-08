using System.Threading.Tasks;

using ManufacturingAPI.Models;

namespace ManufacturingAPI.Services
{
    public interface ICustomerService
    {
        Task<Customer> GetCustomerByIdAsync(string customerId);
    }
}
