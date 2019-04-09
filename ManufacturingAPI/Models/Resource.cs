using Newtonsoft.Json;

namespace ManufacturingAPI.Models
{
    public class Resource : Link
    {
        [JsonIgnore]
        public Link Self { get; set; }
    }
}
