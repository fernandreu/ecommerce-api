using Amazon.DynamoDBv2.DataModel;

using ECommerceAPI.Infrastructure.Data;

namespace ECommerceAPI.Infrastructure.Entries
{
    public class CustomerEntry : BaseEntry
    {
        public const string Prefix = "CUSTOMER-";

        [DynamoDBRangeKey(AttributeName = MainTable.SortKey)]
        public string ContactName { get; set; }

        [DynamoDBProperty(AttributeName = MainTable.DataAttribute)]
        public string Address { get; set; }
    }
}
