using AutoMapper;

using ECommerceAPI.ApplicationCore.Entities;
using ECommerceAPI.ApplicationCore.Interfaces;
using ECommerceAPI.Web.Controllers;
using ECommerceAPI.Web.Resources;

namespace ECommerceAPI.Web.Helpers
{
    public class ResourceMappingProfile : Profile
    {
        public ResourceMappingProfile()
        {
            // AutoMapper needs a public parameterless constructor, even if it isn't of any use. As long as
            // a ResourceMappingProfile instance with the constructor below is added, mapping should work fine
        }

        public ResourceMappingProfile(IProductChecker productChecker)
        {
            this.CreateMap<Customer, CustomerResource>()
                .ForMember(dest => dest.Self, opt => opt.MapFrom(src => Link.To(nameof(CustomersController.GetCustomerById), new { customerId = src.Id })))
                .ForMember(dest => dest.Orders, opt => opt.MapFrom(src => Link.ToCollection(nameof(OrdersController.GetAllOrders), new { customerId = src.Id })));


            this.CreateMap<Order, OrderResource>()
                .ForMember(dest => dest.Self, opt => opt.MapFrom(src => Link.To(nameof(OrdersController.GetOrderById), new { customerId = src.CustomerId, orderId = src.Id })))
                .ForMember(dest => dest.RequiredBinWidth, opt => opt.MapFrom(src => productChecker.CalculateRequiredWidth(src.Products)));
        }
    }
}
