using System.Text.Json.Serialization;

namespace FinCoreBank.Domain.Entities;

public class Transaction
{
    public Guid Id { get; set; }

    public string FromAccount { get; set; } = "";

    public string ToAccount { get; set; } = "";

    public decimal Amount { get; set; }

    public DateTime CreatedOn { get; set; }

    [JsonIgnore]
    public string RequestId { get; set; } = "";
}