using PROG7311_POE.Models;

namespace PROG7311_POE.Observers
{
    public class FinanceObserver : IContractObserver
    {
        public void Update(Contract contract)
        {
            Console.WriteLine($"Finance notified: Contract {contract.ContractId} is now {contract.Status}");
        }
    }
}
