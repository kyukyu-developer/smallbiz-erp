using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ERP.Application.Features.Stock.Commands;
using ERP.Application.Features.Stock.Queries;

namespace ERP.API.Controllers
{
    [ApiController]
    [Route("api/stock-transfers")]
    [Authorize]
    public class StockTransfersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StockTransfersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? fromWarehouseId,
            [FromQuery] string? toWarehouseId,
            [FromQuery] string? productId,
            [FromQuery] int? status,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var query = new GetStockTransfersQuery
            {
                FromWarehouseId = fromWarehouseId,
                ToWarehouseId = toWarehouseId,
                ProductId = productId,
                Status = status,
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
            var query = new GetStockTransferByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return NotFound(result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockTransferCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPatch("{id}/confirm")]
        public async Task<IActionResult> Confirm(string id)
        {
            var command = new ConfirmStockTransferCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpPatch("{id}/cancel")]
        public async Task<IActionResult> Cancel(string id)
        {
            var command = new CancelStockTransferCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }
    }
}
