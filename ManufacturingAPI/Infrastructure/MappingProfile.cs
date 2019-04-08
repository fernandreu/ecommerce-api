using System;

using AutoMapper;

using ManufacturingAPI.Models;

namespace ManufacturingAPI.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<CustomerEntity, Customer>()
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId.Substring(CustomerEntity.Prefix.Length)));

            this.CreateMap<OrderEntity, Order>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId.Substring(OrderEntity.Prefix.Length)))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId.Substring(CustomerEntity.Prefix.Length)));
        }
    }
}
