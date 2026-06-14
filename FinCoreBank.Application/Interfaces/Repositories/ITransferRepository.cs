
using FinCoreBank.Domain.Entities;

namespace FinCoreBank.Application.Interfaces.Repositories;

public interface ITransferRepository
{
    Task SaveAsync(Transaction transaction);

    Task<Transaction?> GetByRequestIdAsync(
        string requestId);

        Task<List<Transaction>>
        GetHistoryAsync();

    Task<Transaction?>
        GetByIdAsync(
            Guid id);
}