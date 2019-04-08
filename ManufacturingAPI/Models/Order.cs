using System.Collections.Generic;

namespace ManufacturingAPI.Models
{
    public class Order
    {
        public string OrderId { get; set; }
        
        public string CustomerId { get; set; }
        
        public string OrderDate { get; set; }

        public List<Product> Products { get; set; }

        public double RequiredBinWidth { get; set; }
    }
}
