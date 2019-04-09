using System.Collections.Generic;
using System.Threading.Tasks;

using ManufacturingAPI.Models;

namespace ManufacturingAPI.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync(string customerId);

        Task<Order> GetOrderByIdAsync(string customerId, string orderId);

        Task<Order> SaveOrderAsync(string customerId, string orderId, Order order);
    }
}
