using System.Collections.Generic;
using System.Threading.Tasks;

using ECommerceAPI.Web.Resources;

namespace ECommerceAPI.Web.Interfaces
{
    /// <summary>
    /// Contains several methods to perform CRUD operations with OrderResource entries
    /// </summary>
    public interface IOrderService
    {
        Task<IEnumerable<OrderResource>> GetAllOrdersAsync(string customerId);

        Task<OrderResource> GetOrderByIdAsync(string customerId, string orderId);

        Task<OrderResource> SaveOrderAsync(string customerId, string orderId, OrderResource orderResource);
    }
}
