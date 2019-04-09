using AutoMapper;

using ManufacturingAPI.Models;
using ManufacturingAPI.Services;

namespace ManufacturingAPI.Extensions
{
    public static class MapperExtensions
    {
        public static Order MapOrderFull(this IMapper mapper, object source, IProductChecker productChecker)
        {
            // TODO: Handle this directly from AutoMapper (will require dependency injection)
            var result = mapper.Map<Order>(source);
            if (result != null)
            {
                result.RequiredBinWidth = productChecker.CalculateRequiredWidth(result.Products);
            }

            return result;
        }
    }
}
