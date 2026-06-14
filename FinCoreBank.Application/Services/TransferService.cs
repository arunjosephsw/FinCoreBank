using FinCoreBank.Application.DTOs;
using FinCoreBank.Application.Interfaces.Repositories;
using FinCoreBank.Application.Interfaces.Services;
using FinCoreBank.Domain.Entities;

namespace FinCoreBank.Application.Services;

public class TransferService
    : ITransferService
{
    private readonly ITransferRepository _repository;

    private readonly IFraudCheckService _fraud;

    public TransferService(
        ITransferRepository repository,
        IFraudCheckService fraud)
    {
        _repository = repository;
        _fraud = fraud;
    }

    public async Task<TransferResponse>
        ProcessAsync(
            TransferRequest request)
    {
        var existing =
            await _repository
                .GetByRequestIdAsync(
                    request.RequestId);

        if(existing != null)
        {
            return new TransferResponse
            {
                Success = true,
                Message = "Duplicate Request"
            };
        }

        if(await _fraud
            .IsFraudulentAsync(request))
        {
            throw new Exception(
                "Fraud detected");
        }

        var transaction =
            new Transaction
            {
                Id = Guid.NewGuid(),
                FromAccount =
                    request.FromAccount,
                ToAccount =
                    request.ToAccount,
                Amount =
                    request.Amount,
                CreatedOn =
                    DateTime.UtcNow,
                RequestId =
                    request.RequestId
            };

        await _repository.SaveAsync(
            transaction);

        return new TransferResponse
        {
            Success = true,
            Message = "Transfer Successful",
            TransactionId =
                transaction.Id
        };
    }

public async Task<List<Transaction>>
    GetHistoryAsync()
{
    return await _repository
        .GetHistoryAsync();
}

public async Task<Transaction?>
    GetByIdAsync(Guid id)
{
    return await _repository
        .GetByIdAsync(id);
}
}