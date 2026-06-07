using System.Net;
using Xunit;

namespace PROG7311.Tests.IntegrationTests
{
    public class ClientsIntegrationTests
    {
        private readonly HttpClient _client;

        public ClientsIntegrationTests()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:8080/")
            };
        }

        [Fact]
        public async Task GetClients_Returns200()
        {
            var response =
                await _client.GetAsync("api/clients");

            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);
        }

        [Fact]
        public async Task GetClients_ReturnsData()
        {
            var response = await _client.GetAsync("api/clients");

            var json = await response.Content.ReadAsStringAsync();

            Assert.False(string.IsNullOrWhiteSpace(json));
        }
    }
}