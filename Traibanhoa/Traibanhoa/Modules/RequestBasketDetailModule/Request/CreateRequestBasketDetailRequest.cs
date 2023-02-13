using FluentValidation;
using System;
using Traibanhoa.Modules.TypeModule.Request;

namespace Traibanhoa.Modules.RequestBasketDetailModule.Request
{
    public class CreateRequestBasketDetailRequest
    {
        public Guid RequestBasketId { get; set; }
        public Guid ProductId { get; set; }
        public int? Quantity { get; set; }
    }
    public class CreateRequestBasketDetailRequestValidator : AbstractValidator<CreateRequestBasketDetailRequest>
    {
        public CreateRequestBasketDetailRequestValidator()
        {
            RuleFor(x => x.RequestBasketId).NotEmpty().NotNull();
            RuleFor(x => x.ProductId).NotEmpty().NotNull();
            RuleFor(x => x.Quantity).NotEmpty().NotNull();
        }
    }
}
