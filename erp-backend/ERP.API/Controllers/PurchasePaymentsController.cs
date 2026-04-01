using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ERP.Application.Features.PurchasePayments.Commands;
using ERP.Application.Features.PurchasePayments.Queries;

namespace ERP.API.Controllers
{
    [ApiController]
    [Route("api/purchases/{invoiceId}/payments")]
    [Authorize]
    public class PurchasePaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PurchasePaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetPayments(string invoiceId)
        {
            var result = await _mediator.Send(new GetPurchasePaymentsQuery { PurchaseInvoiceId = invoiceId });
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> AddPayment(string invoiceId, [FromBody] CreatePurchasePaymentCommand command)
        {
            command.PurchaseInvoiceId = invoiceId;
            var result = await _mediator.Send(command);
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
            return Created($"api/purchases/{invoiceId}/payments/{result.Data!.Id}", result.Data);
        }

        [HttpDelete("{paymentId}")]
        public async Task<IActionResult> DeletePayment(string invoiceId, string paymentId)
        {
            var result = await _mediator.Send(new DeletePurchasePaymentCommand
            {
                PurchaseInvoiceId = invoiceId,
                PaymentId = paymentId
            });
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
            return Ok(result.Data);
        }
    }
}
