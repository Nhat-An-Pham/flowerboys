using FluentValidation;
using System;

namespace Traibanhoa.Modules.SubCateModule.Request
{
    public class CreateSubCategoryRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
    }
    public class CreateSubCategoryRequestValidator : AbstractValidator<CreateSubCategoryRequest>
    {
        public CreateSubCategoryRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.Description).NotEmpty().NotNull();
            RuleFor(x => x.CategoryId).NotEmpty().NotNull();
        }
    }
}
