using Newtonsoft.Json;

namespace ECommerceAPI.Web.Resources
{
    public class BaseResource : Link
    {
        [JsonIgnore]
        public Link Self { get; set; }
    }
}
