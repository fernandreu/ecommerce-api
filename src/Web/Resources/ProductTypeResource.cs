using System.Collections.Generic;

using ECommerceAPI.ApplicationCore.Entities;

using Newtonsoft.Json;

namespace ECommerceAPI.Web.Resources
{
    public class ProductTypeResource : BaseResource
    {
        public string Id { get; set; }
        
        public string Name { get; set; }

        public string Category { get; set; }

        public double Price { get; set; }
        
        public double Weight { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public double Depth { get; set; }
    }
}
