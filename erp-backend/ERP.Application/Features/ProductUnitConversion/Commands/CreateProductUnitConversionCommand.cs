


using ERP.Application.DTOs.Common;


using MediatR;
using ERP.Domain.Enums;
using ERP.Application.DTOs.Units;
using ERP.Application.DTOs.ProductUnitConversion;

namespace ERP.Application.Features.ProductUnitConversion.Commands
{
    public class CreateProductUnitConversionCommand : IRequest<Result<GetProductUnitConversionByIdDto>>
    {
        public string ProductId { get; set; }

        public string FromUnitId { get; set; }

        public string ToUnitId { get; set; }

        public decimal Factor { get; set; }

    }
}

