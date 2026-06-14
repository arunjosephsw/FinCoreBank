using FinCoreBank.Application.DTOs;
using FinCoreBank.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinCoreBank.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TransferController : ControllerBase
{
    private readonly ITransferService _service;

    public TransferController(
        ITransferService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult>
        TransferFunds(
            TransferRequest request)
    {
        var result =
            await _service.ProcessAsync(
                request);

        return Ok(result);
    }

    [HttpGet("history")]
    public async Task<IActionResult>
        GetHistory()
    {
        var result =
            await _service.GetHistoryAsync();

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult>
        GetById(
            Guid id)
    {
        var transaction =
            await _service.GetByIdAsync(
                id);

        if (transaction == null)
            return NotFound();

        return Ok(transaction);
    }
}