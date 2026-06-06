using PROG7311_POE.Models;

namespace PROG7311_POE.Factories
{
    public class PremiumContract : IContract
    {
        public Contract Create(int clientId, DateTime startDate, DateTime endDate, ContractStatus status)
        {
            return new Contract
            {
                ClientId = clientId,
                StartDate = startDate,
                EndDate = endDate,
                ServiceLevel = "Premium",
                Status = status
            };
        }
    }
}
