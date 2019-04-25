using System.Threading.Tasks;

using ECommerceAPI.ApplicationCore.Entities;
using ECommerceAPI.Infrastructure.Entries;
using ECommerceAPI.Infrastructure.Services;
using ECommerceAPI.Web.Resources;

using Xunit;

namespace ECommerceAPI.FunctionalTests.Fixtures
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
            var orders = this.AssertIsJsonResponse<Collection<Order>>(response);
            Assert.NotEmpty(orders.Value);
        }

        [Fact]
        public async Task GetSingleWithCorrectOrderShouldReturn200()
        {
            // Arrange / act
            var response = await this.CallLambdaHandler("Orders_GetSingle_Correct.json");

            // Assert
            Assert.Equal(200, response.StatusCode);
            var order = this.AssertIsJsonResponse<Order>(response);
            Assert.Equal("1", order.Id);
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
            var productChecker = new ProductChecker();
            await this.Parent.Context.DeleteAsync<OrderEntry>(OrderEntry.Prefix + "99", CustomerEntry.Prefix + "TEST");

            // Act
            var response = await this.CallLambdaHandler("Orders_Put_Correct.json");

            // Assert
            Assert.Equal(200, response.StatusCode);
            var results = this.AssertIsJsonResponse<Order>(response);
            Assert.Equal(154.0,  productChecker.CalculateRequiredWidth(results.Products));
        }

        [Fact]
        public async Task PutWithInvalidHeadersShouldReturn400()
        {
            await this.AssertStatusCode("Orders_Put_InvalidHeader.json", 400);
        }
        
        [Fact]
        public async Task PutWithInvalidProductsShouldReturn400()
        {
            await this.AssertStatusCode("Orders_Put_InvalidProducts.json", 400);
        }

        [Fact]
        public async Task PutWithoutProductsShouldReturn400()
        {
            await this.AssertStatusCode("Orders_Put_NoProducts.json", 400);
        }
    }
}
