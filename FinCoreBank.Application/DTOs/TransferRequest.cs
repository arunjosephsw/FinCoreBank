namespace FinCoreBank.Application.DTOs;

public class TransferRequest
{
    public string FromAccount { get; set; } = "";

    public string ToAccount { get; set; } = "";

    public decimal Amount { get; set; }

    public string Otp { get; set; } = "";

    public string RequestId { get; set; } = "";
}