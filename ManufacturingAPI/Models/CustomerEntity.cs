﻿using Amazon.DynamoDBv2.DataModel;

using Newtonsoft.Json;

namespace ManufacturingAPI.Models
{
    [DynamoDBTable(MainTable.Name)]
    public class CustomerEntity
    {
        public const string Prefix = "CUSTOMER-";

        [DynamoDBHashKey(AttributeName = MainTable.PartitionKey)]
        public string CustomerId { get; set; }

        [DynamoDBRangeKey(AttributeName = MainTable.SortKey)]
        public string ContactName { get; set; }

        [DynamoDBProperty(AttributeName = MainTable.DataAttribute)]
        public string Address { get; set; }

        [JsonIgnore]
        public string ResourceCustomerId => this.CustomerId?.Substring(Prefix.Length);
    }
}
