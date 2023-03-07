using FluentValidation;
using System;

namespace Traibanhoa.Modules.SubCateModule.Request
{
    public class UpdateSubCategoryRequest
    {
        public Guid SubCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? CategoryId { get; set; }
        public bool? Status { get; set; }
    }
    public class UpdateSubCategoryRequestValidator : AbstractValidator<UpdateSubCategoryRequest>
    {
        public UpdateSubCategoryRequestValidator()
        {
            RuleFor(x => x.SubCategoryId).NotEmpty().NotNull();
        }
    }
}
