using FinCoreBank.Domain.Entities;
using FinCoreBank.Infrastructure.Data;
using FinCoreBank.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace FinCoreBank.Tests.Repositories;

public class TransferRepositoryTests
{
    private readonly BankingDbContext
        _dbContext;

    private readonly TransferRepository
        _sut;

    public TransferRepositoryTests()
    {
        var options =
            new DbContextOptionsBuilder<BankingDbContext>()
            .UseInMemoryDatabase(
                Guid.NewGuid().ToString())
            .Options;

        _dbContext =
            new BankingDbContext(options);

        _sut =
            new TransferRepository(
                _dbContext);
    }

    [Fact]
    public async Task SaveAsync_Should_Save_Transaction()
    {
        var transaction =
            new Transaction
            {
                Id = Guid.NewGuid(),
                Amount = 1000,
                RequestId = "REQ1"
            };

        await _sut.SaveAsync(
            transaction);

        _dbContext.Transactions
            .Count()
            .Should()
            .Be(1);
    }

    [Fact]
    public async Task GetByRequestIdAsync_Should_Return_Transaction()
    {
        var transaction =
            new Transaction
            {
                Id = Guid.NewGuid(),
                RequestId = "REQ123",
                Amount = 500
            };

        _dbContext.Transactions
            .Add(transaction);

        await _dbContext
            .SaveChangesAsync();

        var result =
            await _sut
                .GetByRequestIdAsync(
                    "REQ123");

        result.Should()
            .NotBeNull();

        result!.Amount
            .Should()
            .Be(500);
    }

    [Fact]
    public async Task GetByRequestIdAsync_Should_Return_Null_When_NotFound()
    {
        var result =
            await _sut
                .GetByRequestIdAsync(
                    "INVALID");

        result.Should()
            .BeNull();
    }
}