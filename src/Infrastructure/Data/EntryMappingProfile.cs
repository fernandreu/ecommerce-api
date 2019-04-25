using AutoMapper;

using ECommerceAPI.ApplicationCore.Entities;
using ECommerceAPI.Infrastructure.Entries;

namespace ECommerceAPI.Infrastructure.Data
{
    public class EntryMappingProfile : Profile
    {
        public EntryMappingProfile()
        {
            this.CreateMap<Customer, CustomerEntry>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => CustomerEntry.Prefix + src.Id));

            this.CreateMap<CustomerEntry, Customer>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Substring(CustomerEntry.Prefix.Length)));

            this.CreateMap<Order, OrderEntry>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => OrderEntry.Prefix + src.Id))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => CustomerEntry.Prefix + src.CustomerId));

            this.CreateMap<OrderEntry, Order>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Substring(OrderEntry.Prefix.Length)))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId.Substring(CustomerEntry.Prefix.Length)));
        }
    }
}
