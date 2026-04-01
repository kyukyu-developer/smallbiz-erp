using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ERP.Application.Features.Stock.Queries;

namespace ERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StockController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StockController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("levels")]
        public async Task<IActionResult> GetStockLevels(
            [FromQuery] string? warehouseId,
            [FromQuery] string? productId,
            [FromQuery] bool? lowStockOnly)
        {
            var query = new GetStockLevelsQuery
            {
                WarehouseId = warehouseId,
                ProductId = productId,
                LowStockOnly = lowStockOnly
            };

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStock(
            [FromQuery] string? warehouseId)
        {
            var query = new GetLowStockQuery
            {
                WarehouseId = warehouseId
            };

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpGet("movements")]
        public async Task<IActionResult> GetStockMovements(
            [FromQuery] string? warehouseId,
            [FromQuery] string? productId,
            [FromQuery] string? movementType,
            [FromQuery] int? referenceType,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var query = new GetStockMovementsQuery
            {
                WarehouseId = warehouseId,
                ProductId = productId,
                MovementType = movementType,
                ReferenceType = referenceType,
                StartDate = startDate,
                EndDate = endDate
            };

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }
    }
}
