using FinCoreBank.Application.DTOs;
using FinCoreBank.Application.Interfaces.Services;

namespace FinCoreBank.Application.Services;

public class FraudCheckService
    : IFraudCheckService
{
    public Task<bool> IsFraudulentAsync(
        TransferRequest request)
    {
        if(request.Amount > 50000)
            return Task.FromResult(true);

        return Task.FromResult(false);
    }
}