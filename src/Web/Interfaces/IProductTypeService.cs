using System.Collections.Generic;
using System.Threading.Tasks;

using ECommerceAPI.Web.Resources;

namespace ECommerceAPI.Web.Interfaces
{
    /// <summary>
    /// Contains several methods to perform CRUD operations with ProductTypeResource entries
    /// </summary>
    public interface IProductTypeService
    {
        Task<IEnumerable<ProductTypeResource>> GetAllProductTypesAsync();

        Task<ProductTypeResource> GetProductTypeByNameAsync(string name);
    }
}
