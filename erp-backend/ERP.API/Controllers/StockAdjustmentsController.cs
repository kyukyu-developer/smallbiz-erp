using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ERP.Application.Features.Stock.Commands;
using ERP.Application.Features.Stock.Queries;

namespace ERP.API.Controllers
{
    [ApiController]
    [Route("api/stock-adjustments")]
    [Authorize]
    public class StockAdjustmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StockAdjustmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? warehouseId,
            [FromQuery] string? productId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var query = new GetStockAdjustmentsQuery
            {
                WarehouseId = warehouseId,
                ProductId = productId,
                StartDate = startDate,
                EndDate = endDate
            };

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var query = new GetStockAdjustmentByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return NotFound(result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockAdjustmentCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
        }
    }
}
