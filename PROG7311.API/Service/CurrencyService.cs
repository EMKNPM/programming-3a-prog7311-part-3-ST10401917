using Newtonsoft.Json;

namespace PROG7311_POE.Service
{
    public class CurrencyService : ICurrencyService
    {
        private readonly HttpClient _httpClient;

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetUsdToZarRate()
        {
            var response = await _httpClient.GetStringAsync("https://api.exchangerate-api.com/v4/latest/USD");
            dynamic data = JsonConvert.DeserializeObject(response);
            return (decimal)data.rates.ZAR;
        }
    }
}
