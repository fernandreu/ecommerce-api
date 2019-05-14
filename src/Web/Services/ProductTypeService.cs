using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using ECommerceAPI.ApplicationCore.Interfaces;
using ECommerceAPI.Web.Interfaces;
using ECommerceAPI.Web.Resources;

namespace ECommerceAPI.Web.Services
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly IProductTypeRepository productTypeRepository;

        private readonly IConfigurationProvider mappingConfiguration;

        public ProductTypeService(IProductTypeRepository productTypeRepository, IConfigurationProvider mappingConfiguration)
        {
            this.productTypeRepository = productTypeRepository;
            this.mappingConfiguration = mappingConfiguration;
        }

        public async Task<IEnumerable<ProductTypeResource>> GetAllProductTypesAsync()
        {
            var results = await this.productTypeRepository.GetAllAsync();
            return results.AsQueryable().ProjectTo<ProductTypeResource>(this.mappingConfiguration);
        }

        public async Task<ProductTypeResource> GetProductTypeByNameAsync(string name)
        {
            var result = await this.productTypeRepository.GetByNameAsync(name);
            var mapper = this.mappingConfiguration.CreateMapper();
            return mapper.Map<ProductTypeResource>(result);
        }
    }
}
