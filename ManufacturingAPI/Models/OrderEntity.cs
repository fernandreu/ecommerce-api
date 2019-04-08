﻿using System.Collections.Generic;

using Amazon.DynamoDBv2.DataModel;

namespace ManufacturingAPI.Models
{
    [DynamoDBTable(MainTable.Name)]
    public class OrderEntity
    {
        public const string Prefix = "ORDER-";
        
        [DynamoDBHashKey(AttributeName = MainTable.PartitionKey)]
        public string OrderId { get; set; }
        
        [DynamoDBRangeKey(AttributeName = MainTable.SortKey)]
        public string CustomerId { get; set; }
        
        [DynamoDBProperty(AttributeName = MainTable.DataAttribute)]
        public string OrderDate { get; set; }

        public List<Product> Products { get; set; }
    }
}