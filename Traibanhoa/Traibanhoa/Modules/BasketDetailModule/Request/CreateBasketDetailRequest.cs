using FluentValidation;
using System;
using Traibanhoa.Modules.TypeModule.Request;

namespace Traibanhoa.Modules.BasketDetailModule.Request
{
    public class CreateBasketDetailRequest
    {
        public Guid BasketId { get; set; }
        public Guid ProductId { get; set; }
        public int? Quantity { get; set; }
    }
    public class CreateBasketDetailRequestValidator : AbstractValidator<CreateBasketDetailRequest>
    {
        public CreateBasketDetailRequestValidator()
        {
            RuleFor(x => x.BasketId).NotEmpty().NotNull();
            RuleFor(x => x.ProductId).NotEmpty().NotNull();
            RuleFor(x => x.Quantity).NotEmpty().NotNull();
        }
    }
}
