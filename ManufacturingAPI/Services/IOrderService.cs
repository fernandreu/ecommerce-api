namespace ManufacturingAPI.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ManufacturingAPI.Models;

    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync(string customerId);

        Task<Order> GetOrderByIdAsync(string customerId, string orderId);

        Task<Order> SaveOrderAsync(string customerId, string orderId, Order order);
    }
}
