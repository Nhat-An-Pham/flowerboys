using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Traibanhoa.Modules.TypeModule.Request
{
    public class CreateTypeRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? Status { get; set; }
    }
    public class CreateTypeRequestValidator : AbstractValidator<CreateTypeRequest>
    {
        public CreateTypeRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.Description).NotEmpty().NotNull();
        }
    }
}
