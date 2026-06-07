using System.Net;
using System.Net.Http.Json;
using Xunit;
using PROG7311_POE.Models;

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
            var response = await _client.GetAsync("api/servicerequests");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetServiceRequests_ReturnsJson()
        {
            var response = await _client.GetAsync("api/servicerequests");
            var json = await response.Content.ReadAsStringAsync();

            Assert.False(string.IsNullOrWhiteSpace(json));
        }

        [Fact]
        public async Task GetServiceRequestById_NonExistentId_Returns404NotFound()
        {
            var response = await _client.GetAsync("api/servicerequests/999999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateServiceRequest_MissingContractId_Returns400BadRequest()
        {
            var invalidRequest = new ServiceRequest
            {
                Description = "No contract assigned test request.",
                CostUSD = 50.00m
            };

            var response = await _client.PostAsJsonAsync("api/servicerequests", invalidRequest);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteServiceRequest_NonExistentId_Returns404NotFound()
        {
            var response = await _client.DeleteAsync("api/servicerequests/999999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}