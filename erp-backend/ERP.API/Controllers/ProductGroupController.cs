using ERP.Application.Features.ProductGroup.Queries;
using ERP.Application.Features.Products.Commands;
using ERP.Application.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace ERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductGroupController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductGroupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetProductGroupQuery          
            {
      
            };

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(string id)
        //{
        //    var query = new GetProductByIdQuery { Id = id };
        //    var result = await _mediator.Send(query);

        //    if (!result.IsSuccess)
        //        return NotFound(result.ErrorMessage);

        //    return Ok(result.Data);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
        //{
        //    var result = await _mediator.Send(command);

        //    if (!result.IsSuccess)
        //        return BadRequest(result.ErrorMessage);

        //    return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(string id, [FromBody] UpdateProductCommand command)
        //{
        //    if (id != command.Id)
        //        return BadRequest("ID mismatch");

        //    var result = await _mediator.Send(command);

        //    if (!result.IsSuccess)
        //        return BadRequest(result.ErrorMessage);

        //    return Ok(result.Data);
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(string id)
        //{
        //    var command = new DeleteProductCommand { Id = id };
        //    var result = await _mediator.Send(command);

        //    if (!result.IsSuccess)
        //        return NotFound(result.ErrorMessage);

        //    return NoContent();
        //}
    }
}
