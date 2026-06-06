namespace PROG7311_POE.Factories
{
    public interface ICurrencyService
    {
        Task<decimal> GetRate();
    }
}
