using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ERP.Application.Features.GoodsReceives.Commands;
using ERP.Application.Features.GoodsReceives.Queries;
using ERP.Domain.Enums;

namespace ERP.API.Controllers
{
    [ApiController]
    [Route("api/goods-receives")]
    [Authorize]
    public class GoodsReceivesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GoodsReceivesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? supplierId,
            [FromQuery] string? warehouseId,
            [FromQuery] GoodsReceiveStatus? status,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var query = new GetGoodsReceivesQuery
            {
                SupplierId = supplierId,
                WarehouseId = warehouseId,
                Status = status,
                StartDate = startDate,
                EndDate = endDate
            };

            var result = await _mediator.Send(query);
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _mediator.Send(new GetGoodsReceiveByIdQuery { Id = id });
            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGoodsReceiveCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPatch("{id}/confirm")]
        public async Task<IActionResult> Confirm(string id)
        {
            var result = await _mediator.Send(new ConfirmGoodsReceiveCommand { Id = id });
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
            return Ok(result.Data);
        }

        [HttpPatch("{id}/cancel")]
        public async Task<IActionResult> Cancel(string id)
        {
            var result = await _mediator.Send(new CancelGoodsReceiveCommand { Id = id });
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
            return Ok(result.Data);
        }
    }
}
