using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ManufacturingAPI.Models;
using ManufacturingAPI.Services;

using Microsoft.AspNetCore.Mvc;

namespace ManufacturingAPI.Controllers
{
    [Route("Customers/{customerId}/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet(Name = nameof(GetAllOrders))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Collection<Order>>> GetAllOrders(string customerId)
        {
            var orders = await this.orderService.GetAllOrdersAsync(customerId);
            if (orders == null)
            {
                // This probably means the customerId was not found
                return this.NotFound();
            }

            return new Collection<Order>
            {
                Self = Link.ToCollection(nameof(this.GetAllOrders), new { customerId }),
                Value = orders.ToArray(),
            };
        }

        [HttpGet("{orderId}", Name = nameof(GetOrderById))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Order>> GetOrderById(string customerId, string orderId)
        {
            var result = await this.orderService.GetOrderByIdAsync(customerId, orderId);
            if (result == null)
            {
                return this.NotFound();
            }

            return result;
        }

        [HttpPut("{orderId}", Name = nameof(SaveOrder))]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Order>> SaveOrder(string customerId, string orderId, [FromBody] Order order)
        {
            var result = await this.orderService.SaveOrderAsync(customerId, orderId, order);
            if (result == null)
            {
                return this.BadRequest();
            }

            return result;
        }
    }
}