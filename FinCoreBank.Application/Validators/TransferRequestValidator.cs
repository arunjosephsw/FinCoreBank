using FinCoreBank.Application.DTOs;
using FluentValidation;

namespace FinCoreBank.Application.Validators;

public class TransferRequestValidator
    : AbstractValidator<TransferRequest>
{
    public TransferRequestValidator()
    {
        RuleFor(x => x.FromAccount)
            .NotEmpty();

        RuleFor(x => x.ToAccount)
            .NotEmpty();

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .LessThanOrEqualTo(100000);

        RuleFor(x => x.Otp)
            .Length(6);

        RuleFor(x => x.RequestId)
            .NotEmpty();
    }
}