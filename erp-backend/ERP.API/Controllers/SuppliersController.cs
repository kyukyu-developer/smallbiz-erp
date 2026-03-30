using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ERP.Application.Features.Suppliers.Queries;
using ERP.Application.Features.Suppliers.Commands;

namespace ERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SuppliersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SuppliersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] bool? includeInactive)
        {
            var query = new GetSuppliersQuery
            {
                SearchTerm = search,
                IncludeInactive = includeInactive
            };

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var query = new GetSupplierByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return NotFound(result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSupplierCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateSupplierCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var command = new DeleteSupplierCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return NotFound(result.ErrorMessage);

            return NoContent();
        }
    }
}
