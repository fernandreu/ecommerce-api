using System;
using System.Threading.Tasks;

using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;

using ManufacturingAPI.Extensions;
using ManufacturingAPI.Models;

namespace ManufacturingAPI.IntegrationTests.Fixtures
{
    public class FixtureParent : IDisposable
    {
        public readonly IAmazonDynamoDB Client;

        public readonly IDynamoDBContext Context;

        private bool disposedValue = false;

        public FixtureParent()
        {
            this.Client = new AmazonDynamoDBClient(RegionEndpoint.EUWest1);

            this.Context = new DynamoDBContext(this.Client);
            
            this.InitializeDatabaseAsync().Wait();
        }
        
        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposedValue)
            {
                return;
            }

            if (disposing)
            {
                // Previously, this was used to delete the table at the end of the test. This can cause delays when
                // the next text fixture in the collection runs, so better to leave that part out
            }

            this.disposedValue = true;
        }
        
        private async Task EnsureTableIsActive()
        {
            var tables = await this.Client.ListTablesAsync();
            if (!tables.TableNames.Contains(MainTable.Name))
            {
                try
                {
                    await this.Client.CreateMainTableAsync();
                }
                catch (Exception)
                {
                    // If a test fixture is not from the same collection as the rest (perhaps due to a mistake), it
                    // might execute in parallel, causing errors when creating the table at the same time
                }
            }

            var describeRequest = new DescribeTableRequest { TableName = MainTable.Name };
            for (;;)
            {
                var response = await this.Client.DescribeTableAsync(describeRequest);
                if (response.Table.TableStatus == TableStatus.ACTIVE)
                {
                    break;
                }

                await Task.Delay(1000);
            }
        }

        private async Task InitializeDatabaseAsync()
        {
            await this.EnsureTableIsActive();
            await SeedData.AddTestDataAsync(this.Context, true);
        }
    }
}
