using Newtonsoft.Json;

namespace ECommerceAPI.Web.Resources
{
    public class CustomerResource : BaseResource
    {
        public Link Orders { get; set; }

        public string Id { get; set; }

        public string ContactName { get; set; }

        public string Address { get; set; }
    }
}
