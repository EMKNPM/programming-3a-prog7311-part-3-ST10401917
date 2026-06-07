using System.Net;
using System.Net.Http.Json;
using Xunit;
using PROG7311_POE.Models;

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
        public async Task GetClients_Returns200OK()
        {
            var response = await _client.GetAsync("api/clients");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetClients_ReturnsData()
        {
            var response = await _client.GetAsync("api/clients");
            var json = await response.Content.ReadAsStringAsync();

            Assert.False(string.IsNullOrWhiteSpace(json));
        }

        [Fact]
        public async Task GetClientById_NonExistentId_Returns404NotFound()
        {
            var response = await _client.GetAsync("api/clients/999999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateClient_MissingRequiredFields_Returns400BadRequest()
        {
            var invalidClient = new Client
            {
                Name = "",
                ContactDetails = "",
                Region = ""
            };

            var response = await _client.PostAsJsonAsync("api/clients", invalidClient);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}