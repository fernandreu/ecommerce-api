using System.Collections.Generic;
using System.Threading.Tasks;

using ManufacturingAPI.Models;

using Microsoft.AspNetCore.Server.Kestrel.Https;

using Xunit;

namespace ManufacturingAPI.IntegrationTests.Fixtures
{
    [Collection(CollectionName)]
    public class OrdersControllerTests : FixtureBase
    {
        public OrdersControllerTests(FixtureParent parent)
            : base(parent)
        {
        }

        [Fact]
        public async Task GetAllWithCorrectCustomerShouldReturn200()
        {
            // Arrange / act
            var response = await this.CallLambdaHandler("Orders_GetAll_Correct.json");

            // Assert
            Assert.Equal(200, response.StatusCode);
            var orders = this.AssertIsJsonResponse<List<Order>>(response);
            Assert.NotEmpty(orders);
        }

        [Fact]
        public async Task GetSingleWithCorrectOrderShouldReturn200()
        {
            // Arrange / act
            var response = await this.CallLambdaHandler("Orders_GetSingle_Correct.json");

            // Assert
            Assert.Equal(200, response.StatusCode);
            var order = this.AssertIsJsonResponse<Order>(response);
            Assert.Equal("1", order.OrderId);
        }

        [Fact]
        public async Task GetAllWithInvalidCustomerShouldReturn404()
        {
            await this.Assert404("Orders_GetAll_InvalidCustomer.json");
        }

        [Fact]
        public async Task GetSingleWithInvalidCustomerShouldReturn404()
        {
            await this.Assert404("Orders_GetSingle_InvalidCustomer.json");
        }

        [Fact]
        public async Task GetSingleWithInvalidOrderShouldReturn404()
        {
            await this.Assert404("Orders_GetSingle_InvalidOrder.json");
        }

        [Fact]
        public async Task PutWithoutExistingItemShouldReturn200()
        {
            // Arrange: ensure no item with the desired ID exists
            await this.Parent.Context.DeleteAsync<OrderEntity>(OrderEntity.Prefix + "99", CustomerEntity.Prefix + "TEST");

            // Act
            var response = await this.CallLambdaHandler("Orders_Put_Correct.json");

            // Assert
            Assert.Equal(200, response.StatusCode);
            var results = this.AssertIsJsonResponse<SaveOrderResults>(response);
            Assert.Equal(42.0, results.BinWidth);
        }

        [Fact]
        public async Task PutWithInvalidHeadersShouldReturn400()
        {
            await this.AssertStatusCode("Orders_Put_InvalidHeader.json", 400);
        }
    }
}
