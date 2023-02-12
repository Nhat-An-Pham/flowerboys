using FluentValidation;
using System;

namespace Traibanhoa.Modules.ProductModule.Request
{
    public class UpdateProductRequest
    {
        public Guid ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? Status { get; set; }
        public string? Picture { get; set; }
        public decimal? Price { get; set; }
        public Guid TypeId { get; set; }
    }
    public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.Description).NotEmpty().NotNull();
            RuleFor(x => x.Picture).NotEmpty().NotNull();
            RuleFor(x => x.TypeId).NotEmpty().NotNull();
            RuleFor(x => x.Price).NotEmpty().NotNull();
        }
    }
}
