using ERP.Application.DTOs.Common;
using ERP.Application.DTOs.ProductUnitConversion;
using MediatR;

namespace ERP.Application.Features.ProductUnitConversion.Commands
{
    public class UpdateProductUnitConversionCommand : IRequest<Result<GetProductUnitConversionByIdDto>>
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string FromUnitId { get; set; }
        public string ToUnitId { get; set; }
        public decimal Factor { get; set; }
        public bool Active { get; set; }
    }
}
