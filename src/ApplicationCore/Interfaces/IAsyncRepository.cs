using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using ECommerceAPI.ApplicationCore.Entities;

namespace ECommerceAPI.ApplicationCore.Interfaces
{
    public interface IAsyncRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(string id);

        Task<IEnumerable<T>> GetAllAsync();

        Task<T> PutAsync(T entity);

        Task<T> PostAsync(T entity);
    }
}
