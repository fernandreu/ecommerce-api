﻿using AutoMapper;

using ECommerceAPI.ApplicationCore.Entities;
using ECommerceAPI.Web.Controllers;
using ECommerceAPI.Web.Resources;

namespace ECommerceAPI.Web.Helpers
{
    public class ResourceMappingProfile : Profile
    {
        public ResourceMappingProfile()
        {
            this.CreateMap<Customer, CustomerResource>()
                .ForMember(dest => dest.Self, opt => opt.MapFrom(src => Link.To(nameof(CustomersController.GetCustomerById), new { customerId = src.Id })))
                .ForMember(dest => dest.Orders, opt => opt.MapFrom(src => Link.ToCollection(nameof(OrdersController.GetAllOrders), new { customerId = src.Id })));


            this.CreateMap<Order, OrderResource>()
                .ForMember(dest => dest.Self, opt => opt.MapFrom(src => Link.To(nameof(OrdersController.GetOrderById), new { customerId = src.CustomerId, orderId = src.Id })))
                .ForMember(dest => dest.RequiredBinWidth, opt => opt.MapFrom<WidthResolver>());

        }
    }
}