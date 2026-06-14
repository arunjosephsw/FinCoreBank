using FinCoreBank.Application.DTOs;

namespace FinCoreBank.Application.Interfaces.Services;

public interface IFraudCheckService
{
    Task<bool> IsFraudulentAsync(
        TransferRequest request);
}