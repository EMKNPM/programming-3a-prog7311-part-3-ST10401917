using Xunit;
using PROG7311_POE.Models;

public class WorkflowTests
{
    [Fact]
    public void CannotCreateRequest_WhenContractExpired()
    {
        // Arrange
        var contract = new Contract
        {
            Status = ContractStatus.Expired
        };

        // Act
        var canCreate = WorkflowValidator.CanCreateRequest(contract);

        // Assert
        Assert.False(canCreate);
    }

    [Fact]
    public void CanCreateRequest_WhenContractActive()
    {
        // Arrange
        var contract = new Contract
        {
            Status = ContractStatus.Active
        };

        // Act
        var canCreate = WorkflowValidator.CanCreateRequest(contract);

        // Assert
        Assert.True(canCreate);
    }
}

// Helper class
public static class WorkflowValidator
{
    public static bool CanCreateRequest(Contract contract)
    {
        return contract.Status != ContractStatus.Expired &&
               contract.Status != ContractStatus.OnHold;
    }
}