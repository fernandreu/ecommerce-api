using AutoMapper;

using ManufacturingAPI.Controllers;
using ManufacturingAPI.Models;

namespace ManufacturingAPI.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<CustomerEntity, Customer>()
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.ResourceCustomerId))
                .ForMember(dest => dest.Self, opt => opt.MapFrom(src => Link.To(nameof(CustomersController.GetCustomerById), new { customerId = src.ResourceCustomerId })))
                .ForMember(dest => dest.Orders, opt => opt.MapFrom(src => Link.ToCollection(nameof(OrdersController.GetAllOrders), new { customerId = src.ResourceCustomerId })));


            this.CreateMap<OrderEntity, Order>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.ResourceOrderId))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.ResourceCustomerId))
                .ForMember(dest => dest.Self, opt => opt.MapFrom(src => Link.To(nameof(OrdersController.GetOrderById), new { customerId = src.ResourceOrderId, orderId = src.ResourceOrderId })));
        }
    }
}
