using ERP.Application.Features.Units.Queries;
using ERP.Application.Features.Warehouses.Commands;
using ERP.Application.Features.Warehouses.Queries;
using ERP.Domain.Enums;
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
        /// Get all warehouses with optional filtering
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
        /// Get warehouse by ID
        /// </summary>
       
    }
}
