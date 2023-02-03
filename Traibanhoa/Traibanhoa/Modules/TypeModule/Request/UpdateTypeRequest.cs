using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Traibanhoa.Modules.TypeModule.Request
{
    public class UpdateTypeRequest
    {
        public Guid TypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? Status { get; set; }
    }
    public class UpdateTypeRequestValidator : AbstractValidator<UpdateTypeRequest>
    {
        public UpdateTypeRequestValidator()
        {
            RuleFor(x => x.TypeId).NotEmpty().NotNull();
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.Description).NotEmpty().NotNull();
        }
    }
}
