using ERP.Shared.Contracts.Common;
using MediatR;

namespace Inventory.Application.Features.Products.Commands;

public class DeleteProductCommand : IRequest<Result<bool>>
{
    public int Id { get; set; }
}
