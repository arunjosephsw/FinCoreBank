using FinCoreBank.Application.DTOs;
using FinCoreBank.Application.Interfaces;
using FinCoreBank.Application.Interfaces.Repositories;
using FinCoreBank.Application.Interfaces.Services;
using FinCoreBank.Application.Services;
using FinCoreBank.Domain.Entities;
using FluentAssertions;
using Moq;

namespace FinCoreBank.Tests.Services;

public class TransferServiceTests
{
    private readonly Mock<ITransferRepository>
        _repositoryMock;

    private readonly Mock<IFraudCheckService>
        _fraudServiceMock;

    private readonly TransferService
        _sut;

    public TransferServiceTests()
    {
        _repositoryMock =
            new Mock<ITransferRepository>();

        _fraudServiceMock =
            new Mock<IFraudCheckService>();

        _sut =
            new TransferService(
                _repositoryMock.Object,
                _fraudServiceMock.Object);
    }

    private TransferRequest CreateRequest()
    {
        return new TransferRequest
        {
            FromAccount = "1001",
            ToAccount = "2001",
            Amount = 1000,
            Otp = "123456",
            RequestId = Guid.NewGuid().ToString()
        };
    }

    [Fact]
    public async Task ProcessAsync_Should_Return_Success_When_Request_Is_Valid()
    {
        // Arrange

        var request =
            CreateRequest();

        _repositoryMock
            .Setup(x =>
                x.GetByRequestIdAsync(
                    request.RequestId))
            .ReturnsAsync(
                (Transaction?)null);

        _fraudServiceMock
            .Setup(x =>
                x.IsFraudulentAsync(
                    request))
            .ReturnsAsync(false);

        // Act

        var result =
            await _sut.ProcessAsync(
                request);

        // Assert

        result.Should().NotBeNull();

        result.Success
            .Should()
            .BeTrue();

        result.Message
            .Should()
            .Be("Transfer Successful");

        _repositoryMock.Verify(
            x => x.SaveAsync(
                It.IsAny<Transaction>()),
            Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_Should_Save_Transaction_When_Request_Is_Valid()
    {
        var request =
            CreateRequest();

        _repositoryMock
            .Setup(x =>
                x.GetByRequestIdAsync(
                    request.RequestId))
            .ReturnsAsync(
                (Transaction?)null);

        _fraudServiceMock
            .Setup(x =>
                x.IsFraudulentAsync(
                    request))
            .ReturnsAsync(false);

        await _sut.ProcessAsync(
            request);

        _repositoryMock.Verify(
            x => x.SaveAsync(
                It.Is<Transaction>(
                    t =>
                        t.Amount ==
                        request.Amount &&
                        t.FromAccount ==
                        request.FromAccount &&
                        t.ToAccount ==
                        request.ToAccount)),
            Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_Should_Return_Duplicate_Request_When_RequestId_Already_Exists()
    {
        var request =
            CreateRequest();

        var existingTransaction =
            new Transaction
            {
                Id = Guid.NewGuid(),
                RequestId =
                    request.RequestId
            };

        _repositoryMock
            .Setup(x =>
                x.GetByRequestIdAsync(
                    request.RequestId))
            .ReturnsAsync(
                existingTransaction);

        var result =
            await _sut.ProcessAsync(
                request);

        result.Success
            .Should()
            .BeTrue();

        result.Message
            .Should()
            .Be("Duplicate Request");

        _repositoryMock.Verify(
            x => x.SaveAsync(
                It.IsAny<Transaction>()),
            Times.Never);

        _fraudServiceMock.Verify(
            x => x.IsFraudulentAsync(
                It.IsAny<TransferRequest>()),
            Times.Never);
    }

    [Fact]
    public async Task ProcessAsync_Should_Throw_Exception_When_Fraud_Is_Detected()
    {
        var request =
            CreateRequest();

        _repositoryMock
            .Setup(x =>
                x.GetByRequestIdAsync(
                    request.RequestId))
            .ReturnsAsync(
                (Transaction?)null);

        _fraudServiceMock
            .Setup(x =>
                x.IsFraudulentAsync(
                    request))
            .ReturnsAsync(true);

        Func<Task> action =
            async () =>
                await _sut.ProcessAsync(
                    request);

        await action.Should()
            .ThrowAsync<Exception>()
            .WithMessage(
                "Fraud detected");

        _repositoryMock.Verify(
            x => x.SaveAsync(
                It.IsAny<Transaction>()),
            Times.Never);
    }

    [Fact]
    public async Task ProcessAsync_Should_Throw_Exception_When_Repository_Save_Fails()
    {
        var request =
            CreateRequest();

        _repositoryMock
            .Setup(x =>
                x.GetByRequestIdAsync(
                    request.RequestId))
            .ReturnsAsync(
                (Transaction?)null);

        _fraudServiceMock
            .Setup(x =>
                x.IsFraudulentAsync(
                    request))
            .ReturnsAsync(false);

        _repositoryMock
            .Setup(x =>
                x.SaveAsync(
                    It.IsAny<Transaction>()))
            .ThrowsAsync(
                new Exception(
                    "Database Failure"));

        Func<Task> action =
            async () =>
                await _sut.ProcessAsync(
                    request);

        await action.Should()
            .ThrowAsync<Exception>()
            .WithMessage(
                "Database Failure");
    }

    [Fact]
    public async Task ProcessAsync_Should_Invoke_Fraud_Service_Exactly_Once()
    {
        var request =
            CreateRequest();

        _repositoryMock
            .Setup(x =>
                x.GetByRequestIdAsync(
                    request.RequestId))
            .ReturnsAsync(
                (Transaction?)null);

        _fraudServiceMock
            .Setup(x =>
                x.IsFraudulentAsync(
                    request))
            .ReturnsAsync(false);

        await _sut.ProcessAsync(
            request);

        _fraudServiceMock.Verify(
            x => x.IsFraudulentAsync(
                request),
            Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_Should_Check_RequestId_Exactly_Once()
    {
        var request =
            CreateRequest();

        _repositoryMock
            .Setup(x =>
                x.GetByRequestIdAsync(
                    request.RequestId))
            .ReturnsAsync(
                (Transaction?)null);

        _fraudServiceMock
            .Setup(x =>
                x.IsFraudulentAsync(
                    request))
            .ReturnsAsync(false);

        await _sut.ProcessAsync(
            request);

        _repositoryMock.Verify(
            x => x.GetByRequestIdAsync(
                request.RequestId),
            Times.Once);
    }
}