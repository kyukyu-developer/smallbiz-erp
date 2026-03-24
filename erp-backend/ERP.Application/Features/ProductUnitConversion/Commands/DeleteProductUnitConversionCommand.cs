using ERP.Application.DTOs.Common;
using MediatR;

namespace ERP.Application.Features.ProductUnitConversion.Commands
{
    public class DeleteProductUnitConversionCommand : IRequest<Result<int>>
    {
        public string Id { get; set; }
    }
}
