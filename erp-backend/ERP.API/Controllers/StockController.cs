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
            [FromQuery] int? productId,
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
    }
}
