using Newtonsoft.Json;

namespace PROG7311_POE.Factories
{
    public class ZARCurrencyService : ICurrencyService
    {
        public async Task<decimal> GetRate()
        {
            using var client = new HttpClient();

            var json = await client.GetStringAsync(
                "https://api.exchangerate-api.com/v4/latest/USD"
            );

            dynamic data = JsonConvert.DeserializeObject(json);
            return (decimal)data.rates.ZAR;
        }
    }
}
