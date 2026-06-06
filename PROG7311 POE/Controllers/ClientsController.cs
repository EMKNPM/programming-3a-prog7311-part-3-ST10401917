using Microsoft.AspNetCore.Mvc;
using PROG7311_POE.Models;
using System;
using System.Net.Http.Json;

namespace PROG7311_POE.Controllers
{
    public class ClientsController : Controller
    {
        private readonly HttpClient _client;

        public ClientsController(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("Backend");
        }

        public async Task<IActionResult> Index()
        {
            var data = await _client.GetFromJsonAsync<List<Client>>("api/clients");
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Client client)
        {
            await _client.PostAsJsonAsync("api/clients", client);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = await _client.GetFromJsonAsync<Client>($"api/clients/{id}");
            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _client.DeleteAsync($"api/clients/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}