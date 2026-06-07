using System.Net;
using System.Net.Http.Json;
using Xunit;
using PROG7311_POE.Models;

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
        public async Task GetContracts_Returns200OK()
        {
            var response = await _client.GetAsync("api/contracts");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetContracts_ReturnsJson()
        {
            var response = await _client.GetAsync("api/contracts");
            var json = await response.Content.ReadAsStringAsync();

            Assert.False(string.IsNullOrWhiteSpace(json));
        }

        [Fact]
        public async Task GetContractById_NonExistentId_Returns404NotFound()
        {
            var response = await _client.GetAsync("api/contracts/999999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateContract_NullRequest_Returns400BadRequest()
        {
            HttpContent emptyContent = new StringContent("", System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/contracts", null);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateStatus_ValidId_Returns200OK()
        {
            var statusPayload = JsonContent.Create(ContractStatus.Expired);
            var response = await _client.PatchAsync("api/contracts/1/status", statusPayload);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return;
            }

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var updatedContract = await response.Content.ReadFromJsonAsync<Contract>();
            Assert.NotNull(updatedContract);
            Assert.Equal(ContractStatus.Expired, updatedContract.Status);
        }

        [Fact]
        public async Task DeleteContract_NonExistentId_Returns404NotFound()
        {
            var response = await _client.DeleteAsync("api/contracts/999999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}