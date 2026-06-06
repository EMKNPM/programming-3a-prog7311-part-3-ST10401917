using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace PROG7311_PART_2.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuthController(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("Backend");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Username and password are required");
                return View();
            }

            try
            {
                var loginRequest = new { username, password };
                var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    
                    // Store JWT token in session
                    HttpContext.Session.SetString("JwtToken", result?.Token ?? "");
                    HttpContext.Session.SetString("Username", username);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Login failed: {ex.Message}");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }

    public class LoginResponse
    {
        public string? Token { get; set; }
    }
}
