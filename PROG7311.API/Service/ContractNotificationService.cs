using PROG7311_POE.Models;
using PROG7311_POE.Observers;

namespace PROG7311_POE.Service
{
    public class ContractNotificationService
    {
        private readonly ContractSubject _subject;

        public ContractNotificationService()
        {
            _subject = new ContractSubject();

            // Attach observers
            _subject.Attach(new FinanceObserver());
            _subject.Attach(new ClientObserver());
        }

        public void NotifyContractChange(Contract contract)
        {
            _subject.Notify(contract);
        }
    }
}
