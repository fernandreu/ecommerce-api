using System.Collections.Generic;
using System.Threading.Tasks;

using ECommerceAPI.Web.Resources;

namespace ECommerceAPI.Web.Interfaces
{
    /// <summary>
    /// Contains several methods to perform CRUD operations with CustomerResource entries
    /// </summary>
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerResource>> GetAllCustomersAsync();

        Task<CustomerResource> GetCustomerByIdAsync(string customerId);
    }
}
