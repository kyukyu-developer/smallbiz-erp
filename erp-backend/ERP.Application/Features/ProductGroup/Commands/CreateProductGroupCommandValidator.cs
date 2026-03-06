
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Application.Features.ProductGroup.Commands
{
    public class CreateProductGroupCommandValidator : AbstractValidator<CreateProductGroupCommand>
    {
        public CreateProductGroupCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product Group name is required")
                .MaximumLength(50).WithMessage("Product Group  name cannot exceed 50 characters");
        }
    }
}
