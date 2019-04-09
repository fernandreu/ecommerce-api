using ManufacturingAPI.Models;

using Microsoft.AspNetCore.Mvc;

namespace ManufacturingAPI.Controllers
{
    [Route("/")]
    [ApiController]
    public class RootController : ControllerBase
    {
        [HttpGet(Name = nameof(GetRoot))]
        public IActionResult GetRoot()
        {
            var response = new RootResponse
            {
                Self = Link.To(nameof(this.GetRoot)),
                Customers = Link.ToCollection(nameof(CustomersController.GetAllCustomers)),
            };

            return this.Ok(response);
        }
    }
}