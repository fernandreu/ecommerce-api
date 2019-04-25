using System.Threading.Tasks;

using ECommerceAPI.ApplicationCore.Entities;

namespace ECommerceAPI.ApplicationCore.Interfaces
{
    public interface ICustomerRepository : IAsyncRepository<Customer>
    {
    }
}
