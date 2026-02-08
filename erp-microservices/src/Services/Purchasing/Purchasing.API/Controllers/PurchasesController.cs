using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Purchasing.Application.DTOs.Purchases;
using Purchasing.Application.Features.Purchases.Commands;
using Purchasing.Application.Features.Purchases.Queries;

namespace Purchasing.API.Controllers;

[ApiController]
[Route("api/purchasing/orders")]
[Authorize]
public class PurchasesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PurchasesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetPurchasesQuery());
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetPurchaseByIdQuery { Id = id });
        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePurchaseDto dto)
    {
        var result = await _mediator.Send(new CreatePurchaseCommand { Dto = dto });
        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
    }

    [HttpPut("{id}/receive")]
    public async Task<IActionResult> Receive(int id, [FromBody] ReceivePurchaseDto dto)
    {
        var result = await _mediator.Send(new ReceivePurchaseCommand { PurchaseId = id, Dto = dto });
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
}
