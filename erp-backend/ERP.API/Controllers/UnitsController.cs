using ERP.Application.Features.Units.Commands;
using ERP.Application.Features.Units.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UnitsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UnitsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all units with optional filtering
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] bool? includeInactive)
        {
            var query = new GetUnitsQuery
            {
                IncludeInactive = includeInactive
            };

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }

        /// <summary>
        /// Get unit by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var query = new GetUnitByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return NotFound(result.ErrorMessage);

            return Ok(result.Data);
        }

        /// <summary>
        /// Create a new unit
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUnitCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
        }

        /// <summary>
        /// Update an existing unit
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUnitCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID in URL does not match ID in request body");

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }

        /// <summary>
        /// Soft-delete a unit (sets Active = false)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var command = new DeleteUnitCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return NotFound(result.ErrorMessage);

            return NoContent();
        }


    }
}
