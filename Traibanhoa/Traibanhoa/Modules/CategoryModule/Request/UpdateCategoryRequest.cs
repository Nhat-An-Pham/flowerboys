using FluentValidation;
using System;

namespace Traibanhoa.Modules.CategoryModule.Request
{
    public class UpdateCategoryRequest
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? Status { get; set; }
    }
    public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
    {
        public UpdateCategoryRequestValidator()
        {
            RuleFor(x => x.CategoryId).NotEmpty().NotNull();
        }
    }
}
