using FluentValidation;
using System;

namespace Traibanhoa.Modules.OrderProductDetailModule.Request
{
    public class UpdateOrderProductDetailRequest
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
    }
    public class UpdateOrderProductDetailRequestValidator : AbstractValidator<UpdateOrderProductDetailRequest>
    {
        public UpdateOrderProductDetailRequestValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty().NotNull();
            RuleFor(x => x.ProductId).NotEmpty().NotNull();
            RuleFor(x => x.Quantity).NotEmpty().NotNull();
            RuleFor(x => x.Price).NotEmpty().NotNull();
        }
    }
}
