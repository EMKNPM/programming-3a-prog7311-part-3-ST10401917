using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using PROG7311_POE.Models;

namespace PROG7311_POE.Controllers
{
    public class ContractsController : Controller
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContractsController(IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor)
        {
            _client = factory.CreateClient("Backend");
            _httpContextAccessor = httpContextAccessor;
        }

        // -------------------------
        // ADD JWT TOKEN
        // -------------------------
        private void AddAuthToken()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");

            _client.DefaultRequestHeaders.Remove("Authorization");

            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // -------------------------
        // INDEX
        // -------------------------
        public async Task<IActionResult> Index()
        {
            AddAuthToken();

            var data = await _client.GetFromJsonAsync<List<ContractReadDto>>("api/contracts");

            return View(data ?? new List<ContractReadDto>());
        }

        // -------------------------
        // CREATE (GET)
        // -------------------------
        public async Task<IActionResult> Create()
        {
            AddAuthToken();

            var clients = await _client.GetFromJsonAsync<List<Client>>("api/clients");

            ViewBag.Clients = clients ?? new List<Client>();

            return View();
        }

        // -------------------------
        // CREATE (POST) - FIXED 415
        // -------------------------
        [HttpPost]
        public async Task<IActionResult> Create(Contract contract, IFormFile? file)
        {
            AddAuthToken();

            try
            {
                using var form = new MultipartFormDataContent();

                form.Add(new StringContent(contract.ClientId.ToString()), "ClientId");
                form.Add(new StringContent(contract.StartDate.ToString("yyyy-MM-dd")), "StartDate");
                form.Add(new StringContent(contract.EndDate.ToString("yyyy-MM-dd")), "EndDate");
                form.Add(new StringContent(contract.Status.ToString()), "Status");
                form.Add(new StringContent(contract.ServiceLevel ?? "Standard"), "ServiceLevel");

                if (file != null && file.Length > 0)
                {
                    var fileStream = file.OpenReadStream();
                    var fileContent = new StreamContent(fileStream);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                    form.Add(fileContent, "file", file.FileName);
                }

                var request = new HttpRequestMessage(HttpMethod.Post, "api/contracts")
                {
                    Content = form
                };

                var response = await _client.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Clear the local Model State dictionary to strip out 
                    // any cached raw 'Contract' references before moving to the DTO page
                    ModelState.Clear();

                    return RedirectToAction(nameof(Index));
                }

                return Content($"STATUS: {response.StatusCode}\nBODY: {body}");
            }
            catch (Exception ex)
            {
                return Content($"ERROR: {ex.Message}");
            }
        }

        // -------------------------
        // DELETE (GET)
        // -------------------------
        public async Task<IActionResult> Delete(int id)
        {
            AddAuthToken();

            var contract = await _client.GetFromJsonAsync<Contract>($"api/contracts/{id}");

            return View(contract);
        }

        // -------------------------
        // DELETE (POST)
        // -------------------------
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            AddAuthToken();

            await _client.DeleteAsync($"api/contracts/{id}");

            return RedirectToAction(nameof(Index));
        }

        // -------------------------
        // DOWNLOAD FILE
        // -------------------------
        public async Task<IActionResult> Download(string path)
        {
            AddAuthToken();

            var bytes = await _client.GetByteArrayAsync($"api/contracts/download?path={path}");

            return File(bytes, "application/pdf", "agreement.pdf");
        }
    }
}