using FluentValidation;
using ERP.Domain.Enums;

namespace ERP.Application.Features.Warehouses.Commands
{
    public class CreateWarehouseCommandValidator : AbstractValidator<CreateWarehouseCommand>
    {
        public CreateWarehouseCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Warehouse name is required")
                .MaximumLength(50).WithMessage("Warehouse name cannot exceed 50 characters");

            RuleFor(x => x.City)
                .MaximumLength(50).WithMessage("City cannot exceed 50 characters");

            RuleFor(x => x.BranchType)
                .IsInEnum().WithMessage("Invalid branch type");

            // ParentWarehouseId is required for Branch and Sub warehouses
            RuleFor(x => x.ParentWarehouseId)
                .NotEmpty().WithMessage("Parent warehouse is required for Branch and Sub warehouses")
                .When(x => x.BranchType == BranchType.Branch || x.BranchType == BranchType.Sub);

            // ParentWarehouseId must be NULL for Main warehouses
            RuleFor(x => x.ParentWarehouseId)
                .Empty().WithMessage("Main warehouse cannot have a parent warehouse")
                .When(x => x.BranchType == BranchType.Main);

            RuleFor(x => x.Location)
                .MaximumLength(100).WithMessage("Location cannot exceed 100 characters");

            RuleFor(x => x.Address)
                .MaximumLength(255).WithMessage("Address cannot exceed 255 characters");

            RuleFor(x => x.Country)
                .MaximumLength(50).WithMessage("Country cannot exceed 50 characters");

            RuleFor(x => x.ContactPerson)
                .MaximumLength(100).WithMessage("Contact person name cannot exceed 100 characters");

            RuleFor(x => x.Phone)
                .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters");
        }
    }
}
