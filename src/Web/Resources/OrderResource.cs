using System.Collections.Generic;

using ECommerceAPI.ApplicationCore.Entities;
using ECommerceAPI.ApplicationCore.Logistics;
using Newtonsoft.Json;

namespace ECommerceAPI.Web.Resources
{
    public class OrderResource : BaseResource
    {
        public string Id { get; set; }
        
        public string CustomerId { get; set; }
        
        public string OrderDate { get; set; }

        public IList<Product> Products { get; set; }

        public OrderDetails Details { get; set; }
    }
}
