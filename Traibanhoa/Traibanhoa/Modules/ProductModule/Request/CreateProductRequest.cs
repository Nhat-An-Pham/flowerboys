using FluentValidation;
using System;
using Traibanhoa.Modules.TypeModule.Request;

namespace Traibanhoa.Modules.ProductModule.Request
{
    public class CreateProductRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? Status { get; set; }
        public string Picture { get; set; }
        public decimal? Price { get; set; }
        public Guid TypeId { get; set; }
        public bool? ForSelling { get; set; }
    }
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.Description).NotEmpty().NotNull();
            RuleFor(x => x.Picture).NotEmpty().NotNull();
            RuleFor(x => x.TypeId).NotEmpty().NotNull();
            RuleFor(x => x.Price).NotEmpty().NotNull();
        }
    }
}
