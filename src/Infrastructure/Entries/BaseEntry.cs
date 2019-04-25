using Amazon.DynamoDBv2.DataModel;

using ECommerceAPI.Infrastructure.Data;

namespace ECommerceAPI.Infrastructure.Entries
{
    [DynamoDBTable(MainTable.Name)]
    public abstract class BaseEntry
    {
        [DynamoDBHashKey(AttributeName = MainTable.PartitionKey)]
        public string Id { get; set; }
        
        public static string GetPrefix<T>()
            where T : BaseEntry
        {
            if (typeof(T) == typeof(OrderEntry))
            {
                return OrderEntry.Prefix;
            }

            if (typeof(T) == typeof(CustomerEntry))
            {
                return CustomerEntry.Prefix;
            }

            return null;
        }
    }
}
