using System.Linq;
using System.Threading.Tasks;

using ECommerceAPI.Web.Helpers;
using ECommerceAPI.Web.Interfaces;
using ECommerceAPI.Web.Resources;

using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Web.Controllers
{
    [Route("[controller]")]
    public class ProductTypesController : ControllerBase
    {
        private readonly IProductTypeService productTypeService;

        public ProductTypesController(IProductTypeService productTypeService)
        {
            this.productTypeService = productTypeService;
        }
        
        [HttpGet(Name = nameof(GetAllProductTypes))]
        [ProducesResponseType(200)]
        public async Task<ActionResult<Collection<ProductTypeResource>>> GetAllProductTypes()
        {
            var productTypes = await this.productTypeService.GetAllProductTypesAsync();
            return new Collection<ProductTypeResource>
            {
                Self = Link.ToCollection(nameof(this.GetAllProductTypes)),
                Value = productTypes.ToArray(),
            };
        }

        [HttpGet("{name}", Name = nameof(GetProductTypeByName))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProductTypeResource>> GetProductTypeByName(string name)
        {
            var productType = await this.productTypeService.GetProductTypeByNameAsync(name);
            if (productType == null)
            {
                return this.NotFound(new ApiError(404, "The name was not found"));
            }

            return productType;
        }
    }
}
