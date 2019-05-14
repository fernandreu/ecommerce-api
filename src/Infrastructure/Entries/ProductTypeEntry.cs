using System.Collections.Generic;

using Amazon.DynamoDBv2.DataModel;

using ECommerceAPI.ApplicationCore.Entities;
using ECommerceAPI.Infrastructure.Data;

namespace ECommerceAPI.Infrastructure.Entries
{
    public class ProductTypeEntry : BaseEntry
    {
        public const string Prefix = "PRODUCT-";
        
        [DynamoDBRangeKey(AttributeName = MainTable.SortKey)]
        public string Name { get; set; }
        
        [DynamoDBProperty(AttributeName = MainTable.DataAttribute)]
        public string Category { get; set; }

        public double Price { get; set; }

        public double Weight { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public double Depth { get; set; }
    }
}
