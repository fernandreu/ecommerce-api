using System.Collections.Generic;

using Newtonsoft.Json;

namespace ManufacturingAPI.Models
{
    public class Order : Resource
    {
        public string OrderId { get; set; }
        
        public string CustomerId { get; set; }
        
        public string OrderDate { get; set; }

        public IList<Product> Products { get; set; }

        public double RequiredBinWidth { get; set; }

        [JsonIgnore]
        public string EntityCustomerId => CustomerEntity.Prefix + this.CustomerId;

        [JsonIgnore]
        public string EntityOrderId => OrderEntity.Prefix + this.CustomerId;
    }
}
