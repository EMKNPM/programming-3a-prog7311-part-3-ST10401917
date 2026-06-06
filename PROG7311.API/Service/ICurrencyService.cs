namespace PROG7311_POE.Service
{
    public interface ICurrencyService
    {
        Task<decimal> GetUsdToZarRate();

    }
}
