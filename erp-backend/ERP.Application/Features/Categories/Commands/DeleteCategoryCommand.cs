using MediatR;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Categories.Commands
{
    public class DeleteCategoryCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
    }
}
