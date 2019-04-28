using AutoMapper;

using ECommerceAPI.ApplicationCore.Entities;
using ECommerceAPI.ApplicationCore.Interfaces;
using ECommerceAPI.Infrastructure.Services;
using ECommerceAPI.Web.Resources;

namespace ECommerceAPI.Web.Helpers
{
    public class WidthResolver : IValueResolver<Order, OrderResource, double>
    {
        private readonly IProductChecker productChecker;

        // TODO: This method is only here due to errors with the dependency injection, and should be removed as soon as possible
        public WidthResolver()
        {
            this.productChecker = new ProductChecker();
        }

        public WidthResolver(IProductChecker productChecker)
        {
            this.productChecker = productChecker;
        }

        public double Resolve(Order source, OrderResource destination, double destMember, ResolutionContext context)
        {
            return this.productChecker.CalculateRequiredWidth(source.Products);
        }
    }
}
