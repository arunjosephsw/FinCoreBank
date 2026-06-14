namespace FinCoreBank.Application.DTOs;

public class TransferResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = "";

    public Guid TransactionId { get; set; }
}