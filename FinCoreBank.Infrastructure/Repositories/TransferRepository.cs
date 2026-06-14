using FinCoreBank.Application.Interfaces.Repositories;
using FinCoreBank.Domain.Entities;
using FinCoreBank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinCoreBank.Infrastructure.Repositories;

public class TransferRepository
    : ITransferRepository
{
    private readonly BankingDbContext _context;

    public TransferRepository(
        BankingDbContext context)
    {
        _context = context;
    }

    public async Task SaveAsync(
        Transaction transaction)
    {
        await _context.Transactions.AddAsync(
            transaction);

        await _context.SaveChangesAsync();
    }

    public async Task<Transaction?>
        GetByRequestIdAsync(
            string requestId)
    {
        return await _context.Transactions
            .FirstOrDefaultAsync(
                x => x.RequestId == requestId);
    }

    public async Task<List<Transaction>>
    GetHistoryAsync()
{
    return await _context.Transactions
        .OrderByDescending(
            x => x.CreatedOn)
        .ToListAsync();
}

public async Task<Transaction?>
    GetByIdAsync(Guid id)
{
    return await _context.Transactions
        .FirstOrDefaultAsync(
            x => x.Id == id);
}
}