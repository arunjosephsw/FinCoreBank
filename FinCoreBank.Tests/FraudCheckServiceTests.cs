using FinCoreBank.Application.DTOs;
using FinCoreBank.Application.Services;
using FluentAssertions;

namespace FinCoreBank.Tests.Services;

public class FraudCheckServiceTests
{
    private readonly FraudCheckService _sut;

    public FraudCheckServiceTests()
    {
        _sut =
            new FraudCheckService();
    }

    [Fact]
    public async Task IsFraudulentAsync_Should_ReturnFalse_When_AmountBelowLimit()
    {
        var request =
            new TransferRequest
            {
                Amount = 1000
            };

        var result =
            await _sut.IsFraudulentAsync(
                request);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task IsFraudulentAsync_Should_ReturnTrue_When_AmountAboveLimit()
    {
        var request =
            new TransferRequest
            {
                Amount = 75000
            };

        var result =
            await _sut.IsFraudulentAsync(
                request);

        result.Should().BeTrue();
    }
}