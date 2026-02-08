using Inventory.Application.Features.Stock.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers;

[ApiController]
[Route("api/inventory/stock")]
[Authorize]
public class StockController : ControllerBase
{
    private readonly IMediator _mediator;

    public StockController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetStock(int productId)
    {
        var result = await _mediator.Send(new GetWarehouseStockQuery { ProductId = productId });
        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet("check-availability")]
    public async Task<IActionResult> CheckAvailability(
        [FromQuery] int productId,
        [FromQuery] int warehouseId,
        [FromQuery] decimal requiredQuantity)
    {
        var result = await _mediator.Send(new CheckStockAvailabilityQuery
        {
            ProductId = productId,
            WarehouseId = warehouseId,
            RequiredQuantity = requiredQuantity
        });

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }
}
