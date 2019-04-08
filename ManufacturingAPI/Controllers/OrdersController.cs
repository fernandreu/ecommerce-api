using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ManufacturingAPI.Models;
using ManufacturingAPI.Services;

using Microsoft.AspNetCore.Mvc;

namespace ManufacturingAPI.Controllers
{
    [Route("customers/{customerId}/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders(string customerId)
        {
            var result = await this.orderService.GetAllOrdersAsync(customerId);
            return result == null ? this.NotFound() : new ActionResult<IEnumerable<Order>>(result);
        }

        [HttpGet("{orderId}")]
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

        [HttpPut("{orderId}")]
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