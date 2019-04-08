using System.IO;
using System.Threading.Tasks;

using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;

using Newtonsoft.Json;

using Xunit;

namespace ManufacturingAPI.IntegrationTests.Fixtures
{
    public class RootControllerTests
    {
        [Fact]
        public async Task GetRootShouldReturn200()
        {
            // Arrange
            var lambdaFunction = new LambdaEntryPoint();
            var requestStr = File.ReadAllText("./SampleRequests/Root_Get.json");
            var request = JsonConvert.DeserializeObject<APIGatewayProxyRequest>(requestStr);
            var context = new TestLambdaContext();

            // Act
            var response = await lambdaFunction.FunctionHandlerAsync(request, context);

            // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.True(response.MultiValueHeaders.ContainsKey("Content-Type"));
            Assert.Equal("application/json; charset=utf-8", response.MultiValueHeaders["Content-Type"][0]);
        }
    }
}
