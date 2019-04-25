using System.IO;
using System.Threading.Tasks;

using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;

using ECommerceAPI.Web;

using Newtonsoft.Json;

using Xunit;

namespace ECommerceAPI.FunctionalTests.Fixtures
{
    public class FixtureBase : IClassFixture<FixtureParent>
    {
        public const string CollectionName = "RunInSeries";

        public FixtureBase(FixtureParent parent)
        {
            this.Parent = parent;
        }

        protected FixtureParent Parent { get; }

        protected async Task<APIGatewayProxyResponse> CallLambdaHandler(string fileName)
        {
            // This will typically be the 'Arrange' part of the test
            var lambdaFunction = new LambdaEntryPoint();
            var requestStr = File.ReadAllText($"./SampleRequests/{fileName}");
            var request = JsonConvert.DeserializeObject<APIGatewayProxyRequest>(requestStr);
            var context = new TestLambdaContext();

            // This will typically be the 'Act' part of the test
            var response = await lambdaFunction.FunctionHandlerAsync(request, context);
            return response;
        }

        protected void AssertIsJsonResponse(APIGatewayProxyResponse response)
        {
            Assert.True(response.MultiValueHeaders.ContainsKey("Content-Type"));
            Assert.Equal("application/json; charset=utf-8", response.MultiValueHeaders["Content-Type"][0]);
        }
        
        protected T AssertIsJsonResponse<T>(APIGatewayProxyResponse response)
        {
            this.AssertIsJsonResponse(response);
            return JsonConvert.DeserializeObject<T>(response.Body);
        }

        protected async Task AssertStatusCode(string fileName, int statusCode)
        {
            // Arrange / act
            var response = await this.CallLambdaHandler(fileName);

            // Assert
            Assert.Equal(statusCode, response.StatusCode);
            this.AssertIsJsonResponse(response);
        }

        protected async Task Assert404(string fileName)
        {
            await this.AssertStatusCode(fileName, 404);
        }
    }
}
