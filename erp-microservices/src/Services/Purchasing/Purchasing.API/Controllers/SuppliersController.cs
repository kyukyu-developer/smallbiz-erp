using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Purchasing.Application.DTOs.Suppliers;
using Purchasing.Application.Features.Suppliers.Commands;
using Purchasing.Application.Features.Suppliers.Queries;

namespace Purchasing.API.Controllers;

[ApiController]
[Route("api/purchasing/suppliers")]
[Authorize]
public class SuppliersController : ControllerBase
{
    private readonly IMediator _mediator;

    public SuppliersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetSuppliersQuery());
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSupplierDto dto)
    {
        var result = await _mediator.Send(new CreateSupplierCommand { Dto = dto });
        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetAll), new { id = result.Data!.Id }, result);
    }
}
