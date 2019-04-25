using Amazon.DynamoDBv2.DataModel;

using AutoMapper;

using ECommerceAPI.ApplicationCore.Entities;
using ECommerceAPI.ApplicationCore.Interfaces;
using ECommerceAPI.Infrastructure.Entries;

namespace ECommerceAPI.Infrastructure.Services
{
    public class CustomerRepository : BaseRepository<Customer, CustomerEntry>, ICustomerRepository
    {
        public CustomerRepository(IDynamoDBContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
