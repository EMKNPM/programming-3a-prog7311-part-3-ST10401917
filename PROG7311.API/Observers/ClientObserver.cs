using PROG7311_POE.Models;

namespace PROG7311_POE.Observers
{
    public class ClientObserver : IContractObserver
    {
        public void Update(Contract contract)
        {
            Console.WriteLine($"Client notified: Your contract is now {contract.Status}");
        }
    }
}
