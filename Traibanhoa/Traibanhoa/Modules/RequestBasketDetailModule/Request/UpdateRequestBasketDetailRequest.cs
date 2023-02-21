using FluentValidation;
using System;

namespace Traibanhoa.Modules.RequestBasketDetailModule.Request
{
    public class UpdateRequestBasketDetailRequest
    {
        public Guid RequestBasketId { get; set; }
        public Guid ProductId { get; set; }
        public int? Quantity { get; set; }
    }
    public class UpdateRequestBasketRequestDetailValidator : AbstractValidator<UpdateRequestBasketDetailRequest>
    {
        public UpdateRequestBasketRequestDetailValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty().NotNull();
            RuleFor(x => x.Quantity).NotEmpty().NotNull();
        }
    }
}
