
namespace PROG7311_POE.Factories
{
    public class AfricaFactory : IRegionalFactory
    {
        public ICurrencyService CreateCurrencyService()
        {
            return new ZARCurrencyService();
        }
    }
}
