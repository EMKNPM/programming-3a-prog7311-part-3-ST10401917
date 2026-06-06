using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using PROG7311_POE.Models;

namespace PROG7311_POE.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly HttpClient _client;

        public ServiceRequestsController(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("Backend");
        }

        public async Task<IActionResult> Index()
        {
            var data = await _client.GetFromJsonAsync<List<ServiceRequest>>("api/servicerequests");
            return View(data);
        }

        public async Task<IActionResult> Create()
        {
            List<Contract> contracts = new();

            try
            {
                var response = await _client.GetAsync("api/contracts?status=Active");

                if (response.IsSuccessStatusCode)
                {
                    contracts = await response.Content.ReadFromJsonAsync<List<Contract>>()
                                ?? new List<Contract>();
                }
                else
                {
                    contracts = new List<Contract>();
                }
            }
            catch
            {
                contracts = new List<Contract>();
            }

            ViewBag.Contracts = contracts;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ServiceRequest request)
        {
            var response = await _client.PostAsJsonAsync("api/servicerequests", request);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Failed to create Service Request");
                return View(request);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var request = await _client.GetFromJsonAsync<ServiceRequest>($"api/servicerequests/{id}");
            return View(request);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _client.DeleteAsync($"api/servicerequests/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}