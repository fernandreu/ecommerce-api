using System.Net;

using ECommerceAPI.Web.Helpers;

using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet("{statusCode}", Name = nameof(HandleStatusCode))]
        [HttpPut("{statusCode}", Name = nameof(HandleStatusCode))]
        public ActionResult<ApiError> HandleStatusCode(int statusCode)
        {
            var parsedCode = (HttpStatusCode)statusCode;
            var error = new ApiError(statusCode, parsedCode.ToString());
            return error;
        }
    }
}