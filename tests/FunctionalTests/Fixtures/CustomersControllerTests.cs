using System.Threading.Tasks;

using ECommerceAPI.ApplicationCore.Entities;

using Xunit;

namespace ECommerceAPI.FunctionalTests.Fixtures
{
    [Collection(CollectionName)]
    public class CustomerControllerTests : FixtureBase
    {
        public CustomerControllerTests(FixtureParent parent)
            : base(parent)
        {
        }

        [Fact]
        public async Task GetCorrectCustomerShouldReturn200()
        {
            // Arrange / act
            var response = await this.CallLambdaHandler("Customers_Get_Correct.json");

            // Assert
            Assert.Equal(200, response.StatusCode);
            var customer = this.AssertIsJsonResponse<Customer>(response);
            Assert.Equal("TEST", customer.Id);
        }

        [Fact]
        public async Task GetInvalidCustomerShouldReturn404()
        {
            await this.Assert404("Customers_Get_Invalid.json");
        }
    }
}
