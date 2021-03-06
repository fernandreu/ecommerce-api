﻿using ECommerceAPI.Web.Resources;

using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Web.Controllers
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
                ProductTypes = Link.ToCollection(nameof(ProductTypesController.GetAllProductTypes))
            };

            return this.Ok(response);
        }
    }
}