using System.Net;
using Xunit;

namespace PROG7311.Tests.IntegrationTests
{
    public class ServiceRequestsIntegrationTests
    {
        private readonly HttpClient _client;

        public ServiceRequestsIntegrationTests()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:8080/")
            };
        }

        [Fact]
        public async Task GetServiceRequests_Returns200()
        {
            var response =
                await _client.GetAsync("api/servicerequests");

            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);
        }

        [Fact]
        public async Task GetServiceRequests_ReturnsJson()
        {
            var response =
                await _client.GetAsync("api/servicerequests");

            var json =
                await response.Content.ReadAsStringAsync();

            Assert.False(string.IsNullOrWhiteSpace(json));
        }
    }
}