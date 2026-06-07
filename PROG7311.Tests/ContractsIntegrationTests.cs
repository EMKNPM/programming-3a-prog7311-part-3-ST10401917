using System.Net;
using Xunit;

namespace PROG7311.Tests.IntegrationTests
{
    public class ContractsIntegrationTests
    {
        private readonly HttpClient _client;

        public ContractsIntegrationTests()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:8080/")
            };
        }

        [Fact]
        public async Task GetContracts_Returns200()
        {
            var response =
                await _client.GetAsync("api/contracts");

            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);
        }

        [Fact]
        public async Task GetContracts_ReturnsJson()
        {
            var response =
                await _client.GetAsync("api/contracts");

            var json =
                await response.Content.ReadAsStringAsync();

            Assert.False(string.IsNullOrWhiteSpace(json));
        }
    }
}