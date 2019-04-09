using Microsoft.AspNetCore.Mvc;

namespace ManufacturingAPI.Controllers
{
    [Route("/")]
    [ApiController]
    public class RootController : ControllerBase
    {
        [HttpGet]
        public object Index()
        {
            return new
            {
                Customers = "TODO/Link/To/Customers",
                Orders = "TODO/Link/To/Orders",
            };
        }
    }
}