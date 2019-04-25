using System;
using System.Linq;
using System.Threading.Tasks;

using ECommerceAPI.Web.Helpers;
using ECommerceAPI.Web.Interfaces;
using ECommerceAPI.Web.Resources;

using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Web.Controllers
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
        public async Task<ActionResult<Collection<OrderResource>>> GetAllOrders(string customerId)
        {
            var orders = await this.orderService.GetAllOrdersAsync(customerId);
            if (orders == null)
            {
                return this.NotFound(new ApiError(404, "The customerId was not found"));
            }

            return new Collection<OrderResource>
            {
                Self = Link.ToCollection(nameof(this.GetAllOrders), new { customerId }),
                Value = orders.ToArray(),
            };
        }

        [HttpGet("{orderId}", Name = nameof(GetOrderById))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<OrderResource>> GetOrderById(string customerId, string orderId)
        {
            var result = await this.orderService.GetOrderByIdAsync(customerId, orderId);
            if (result == null)
            {
                return this.NotFound(new ApiError(404, "There combination of customerId and orderId specified was not found"));
            }

            return result;
        }

        [HttpPut("{orderId}", Name = nameof(SaveOrder))]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<OrderResource>> SaveOrder(string customerId, string orderId, [FromBody] OrderResource orderResource)
        {
            try
            {
                return await this.orderService.SaveOrderAsync(customerId, orderId, orderResource);
            }
            catch (ArgumentException exception)
            {
                return this.BadRequest(new ApiError(400, exception.Message));
            }
        }
    }
}