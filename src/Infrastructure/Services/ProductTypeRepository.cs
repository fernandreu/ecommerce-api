using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using AutoMapper;

using ECommerceAPI.ApplicationCore.Entities;
using ECommerceAPI.ApplicationCore.Interfaces;
using ECommerceAPI.Infrastructure.Entries;

namespace ECommerceAPI.Infrastructure.Services
{
    public class ProductTypeRepository : BaseRepository<ProductType, ProductTypeEntry>, IProductTypeRepository
    {
        public ProductTypeRepository(IDynamoDBContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public async Task<ProductType> GetByNameAsync(string name)
        {
            var results = await this.Context.ScanAsync<ProductTypeEntry>(new[]
            {
                new ScanCondition(nameof(ProductTypeEntry.Id), ScanOperator.BeginsWith, this.Prefix),
                new ScanCondition(nameof(ProductTypeEntry.Name), ScanOperator.Equal, name), 
            }).GetNextSetAsync();

            var first = results.FirstOrDefault();
            return first == null ? null : this.Mapper.Map<ProductType>(first);
        }
    }
}
