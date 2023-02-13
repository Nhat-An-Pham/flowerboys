using FluentValidation;
using System;

namespace Traibanhoa.Modules.BasketDetailModule.Request
{
    public class UpdateBasketDetailRequest
    {
        public Guid BasketId { get; set; }
        public Guid ProductId { get; set; }
        public int? Quantity { get; set; }
    }
    public class UpdateBasketDetailRequestValidator : AbstractValidator<UpdateBasketDetailRequest>
    {
        public UpdateBasketDetailRequestValidator()
        {
            RuleFor(x => x.BasketId).NotEmpty().NotNull();
            RuleFor(x => x.ProductId).NotEmpty().NotNull();
            RuleFor(x => x.Quantity).NotEmpty().NotNull();
        }
    }
}
