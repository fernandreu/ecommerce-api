using System.Threading.Tasks;

using Amazon.DynamoDBv2.DataModel;

using AutoMapper;

using ECommerceAPI.ApplicationCore.Entities;
using ECommerceAPI.ApplicationCore.Interfaces;
using ECommerceAPI.Infrastructure.Entries;

namespace ECommerceAPI.Infrastructure.Services
{
    public class OrderRepository : BaseRepository<Order, OrderEntry>, IOrderRepository
    {
        public OrderRepository(IDynamoDBContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
