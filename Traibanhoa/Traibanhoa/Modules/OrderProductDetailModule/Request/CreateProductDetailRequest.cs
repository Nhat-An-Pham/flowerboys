using FluentValidation;
using System;
using Traibanhoa.Modules.TypeModule.Request;

namespace Traibanhoa.Modules.OrderProductDetailModule.Request
{
    public class CreateOrderProductDetailRequest
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
    }
    public class CreateOrderProductDetailRequestValidator : AbstractValidator<CreateOrderProductDetailRequest>
    {
        public CreateOrderProductDetailRequestValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty().NotNull();
            RuleFor(x => x.ProductId).NotEmpty().NotNull();
            RuleFor(x => x.Quantity).NotEmpty().NotNull();
            RuleFor(x => x.Price).NotEmpty().NotNull();
        }
    }
}
