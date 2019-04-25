using System.Collections.Generic;
using System.Threading.Tasks;

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

using ECommerceAPI.Infrastructure.Data;

namespace ECommerceAPI.Infrastructure.Extensions
{
    public static class DynamoDbExtensions
    {
        /// <summary>
        /// Creates a DynamoDB table, following the same structure as defined in the AWS CloudFormation template
        /// </summary>
        /// <param name="client">The DynamoDB client that will be used to create the table</param>
        /// <returns>An awaitable task</returns>
        public static async Task CreateMainTableAsync(this IAmazonDynamoDB client)
        {
            var keySchema = new List<KeySchemaElement>
            {
                new KeySchemaElement(MainTable.PartitionKey, KeyType.HASH),
                new KeySchemaElement(MainTable.SortKey, KeyType.RANGE),
            };

            var attributeDefinitions = new List<AttributeDefinition>
            {
                new AttributeDefinition(MainTable.PartitionKey, ScalarAttributeType.S),
                new AttributeDefinition(MainTable.SortKey, ScalarAttributeType.S),
                new AttributeDefinition(MainTable.DataAttribute, ScalarAttributeType.S),
            };
            
            var gsi = new GlobalSecondaryIndex
            {
                IndexName = "GSI-1",
                ProvisionedThroughput = new ProvisionedThroughput(10, 1),
                Projection = new Projection { ProjectionType = ProjectionType.ALL },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement(MainTable.SortKey, KeyType.HASH),
                    new KeySchemaElement(MainTable.DataAttribute, KeyType.RANGE),
                }
            };

            var request = new CreateTableRequest
            {
                TableName = MainTable.Name,
                ProvisionedThroughput = new ProvisionedThroughput(5, 1),
                AttributeDefinitions = attributeDefinitions,
                KeySchema = keySchema,
                GlobalSecondaryIndexes = { gsi }
            };
            
            // If the table exists, delete it first
            var tables = await client.ListTablesAsync();
            if (tables.TableNames.Contains(MainTable.Name))
            {
                await client.DeleteTableAsync(MainTable.Name);
            }

            await client.CreateTableAsync(request);
        }
    }
}
