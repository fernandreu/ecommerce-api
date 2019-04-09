using Newtonsoft.Json;

namespace ManufacturingAPI.Models
{
    public class Customer : Resource
    {
        public Link Orders { get; set; }

        public string CustomerId { get; set; }

        public string ContactName { get; set; }

        public string Address { get; set; }

        [JsonIgnore]
        public string EntityCustomerId => CustomerEntity.Prefix + this.CustomerId;
    }
}
