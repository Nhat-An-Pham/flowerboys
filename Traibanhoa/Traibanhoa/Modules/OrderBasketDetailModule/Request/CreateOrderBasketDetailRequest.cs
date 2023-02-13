using FluentValidation;
using System;
using Traibanhoa.Modules.TypeModule.Request;

namespace Traibanhoa.Modules.OrderBasketDetailModule.Request
{
    public class CreateOrderBasketDetailRequest
    {
        public Guid OrderId { get; set; }
        public Guid BasketId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public bool? IsRequest { get; set; }
    }
    public class CreateOrderBasketDetailRequestValidator : AbstractValidator<CreateOrderBasketDetailRequest>
    {
        public CreateOrderBasketDetailRequestValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty().NotNull();
            RuleFor(x => x.BasketId).NotEmpty().NotNull();
            RuleFor(x => x.Quantity).NotEmpty().NotNull();
            RuleFor(x => x.Price).NotEmpty().NotNull();
            RuleFor(x => x.IsRequest).NotEmpty().NotNull();
        }
    }
}
