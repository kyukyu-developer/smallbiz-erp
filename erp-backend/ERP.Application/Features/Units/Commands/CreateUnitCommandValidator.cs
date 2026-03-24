using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;





namespace ERP.Application.Features.Units.Commands
{
    public class CreateUnitCommandValidator : AbstractValidator<CreateUnitCommand>
    {
        public CreateUnitCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Unit name is required")
                .MaximumLength(50).WithMessage("Unit name cannot exceed 50 characters");

            RuleFor(x => x.Symbol)
               .NotEmpty().WithMessage("Symbol name is required");
        }
    }
}
