using System.Collections.Generic;

using Amazon.DynamoDBv2.DataModel;

using ECommerceAPI.ApplicationCore.Entities;
using ECommerceAPI.Infrastructure.Data;

namespace ECommerceAPI.Infrastructure.Entries
{
    public class OrderEntry : BaseEntry
    {
        public const string Prefix = "ORDER-";
        
        [DynamoDBRangeKey(AttributeName = MainTable.SortKey)]
        public string CustomerId { get; set; }
        
        [DynamoDBProperty(AttributeName = MainTable.DataAttribute)]
        public string OrderDate { get; set; }

        public List<Product> Products { get; set; }
    }
}
