using System.Threading.Tasks;
using ECommerceAPI.ApplicationCore.Entities;

namespace ECommerceAPI.ApplicationCore.Interfaces
{
    public interface IProductTypeRepository : IAsyncRepository<ProductType>
    {
        Task<ProductType> GetByNameAsync(string name);
    }
}
