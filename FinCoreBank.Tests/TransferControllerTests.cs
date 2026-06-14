using FinCoreBank.API.Controllers;
using FinCoreBank.Application.DTOs;
using FinCoreBank.Application.Interfaces.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FinCoreBank.Tests.Controllers;

public class TransferControllerTests
{
    private readonly Mock<ITransferService>
        _transferServiceMock;

    private readonly TransferController
        _sut;

    public TransferControllerTests()
    {
        _transferServiceMock =
            new Mock<ITransferService>();

        _sut =
            new TransferController(
                _transferServiceMock.Object);
    }

    [Fact]
    public async Task TransferFunds_Should_Return_OkResult_When_TransferSuccessful()
    {
        var request =
            new TransferRequest
            {
                FromAccount = "1001",
                ToAccount = "2001",
                Amount = 1000,
                Otp = "123456",
                RequestId = Guid.NewGuid().ToString()
            };

        var response =
            new TransferResponse
            {
                Success = true,
                Message = "Success"
            };

        _transferServiceMock
            .Setup(x =>
                x.ProcessAsync(request))
            .ReturnsAsync(response);

        var result =
            await _sut.TransferFunds(
                request);

        result.Should()
            .BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task TransferFunds_Should_ThrowException_When_ServiceFails()
    {
        _transferServiceMock
            .Setup(x =>
                x.ProcessAsync(
                    It.IsAny<TransferRequest>()))
            .ThrowsAsync(
                new Exception(
                    "Database Error"));

        Func<Task> action =
            async () =>
                await _sut.TransferFunds(
                    new TransferRequest());

        await action.Should()
            .ThrowAsync<Exception>()
            .WithMessage("Database Error");
    }

    [Fact]
    public async Task TransferFunds_Should_Invoke_Service_ExactlyOnce()
    {
        var request =
            new TransferRequest();

        _transferServiceMock
            .Setup(x =>
                x.ProcessAsync(request))
            .ReturnsAsync(
                new TransferResponse());

        await _sut.TransferFunds(
            request);

        _transferServiceMock.Verify(
            x => x.ProcessAsync(request),
            Times.Once);
    }
}