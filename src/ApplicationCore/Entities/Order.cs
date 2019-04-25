using System.Collections.Generic;

namespace ECommerceAPI.ApplicationCore.Entities
{
    public class Order : BaseEntity
    {
        public string CustomerId { get; set; }
        
        public string OrderDate { get; set; }

        public IList<Product> Products { get; set; }
    }
}
