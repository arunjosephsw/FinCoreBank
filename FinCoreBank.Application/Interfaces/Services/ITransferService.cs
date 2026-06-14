using FinCoreBank.Application.DTOs;
using FinCoreBank.Domain.Entities;

namespace FinCoreBank.Application.Interfaces.Services;

public interface ITransferService
{
    Task<TransferResponse> ProcessAsync(
        TransferRequest request);
        
        Task<List<Transaction>>
        GetHistoryAsync();

    Task<Transaction?>
        GetByIdAsync(
            Guid id);
}