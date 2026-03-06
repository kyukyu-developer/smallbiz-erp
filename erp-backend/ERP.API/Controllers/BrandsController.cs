

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ERP.Application.Features.Brands.Queries;


namespace ERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BrandsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BrandsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetBrandsQuery
            {
               
            };

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(int id)
        //{
        //    var query = new GetCategoryByIdQuery { Id = id };
        //    var result = await _mediator.Send(query);

        //    if (!result.IsSuccess)
        //        return NotFound(result.ErrorMessage);

        //    return Ok(result.Data);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
        //{
        //    var result = await _mediator.Send(command);

        //    if (!result.IsSuccess)
        //        return BadRequest(result.ErrorMessage);

        //    return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryCommand command)
        //{
        //    if (id != command.Id)
        //        return BadRequest("ID mismatch");

        //    var result = await _mediator.Send(command);

        //    if (!result.IsSuccess)
        //        return BadRequest(result.ErrorMessage);

        //    return Ok(result.Data);
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var command = new DeleteCategoryCommand { Id = id };
        //    var result = await _mediator.Send(command);

        //    if (!result.IsSuccess)
        //        return NotFound(result.ErrorMessage);

        //    return NoContent();
        //}
    }
}
