using PROG7311_POE.Models;

namespace PROG7311_POE.Factories
{
    public interface IContract
    {
        Contract Create(int clientId, DateTime startDate, DateTime endDate, ContractStatus status);

    }
}
