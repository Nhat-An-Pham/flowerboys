using FluentValidation;
using System;
using Traibanhoa.Modules.TypeModule.Request;

namespace Traibanhoa.Modules.OrderModule.Request
{
    public class CreateOrderRequest
    {
        public Guid OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public string ShippedAddress { get; set; }
        public string Phonenumber { get; set; }
        public string Email { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? OrderStatus { get; set; }
        public Guid? OrderBy { get; set; }
        public Guid? ConfirmBy { get; set; }
    }
    public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderRequestValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty().NotNull();
            RuleFor(x => x.OrderDate).NotEmpty().NotNull();
            RuleFor(x => x.ShippedDate).NotEmpty().NotNull();
            RuleFor(x => x.ShippedAddress).NotEmpty().NotNull();
            RuleFor(x => x.Phonenumber).NotEmpty().NotNull();
            RuleFor(x => x.Email).NotEmpty().NotNull();
            RuleFor(x => x.TotalPrice).NotEmpty().NotNull();
            RuleFor(x => x.OrderStatus).NotEmpty().NotNull();
            RuleFor(x => x.OrderBy).NotEmpty().NotNull();
            RuleFor(x => x.ConfirmBy).NotEmpty().NotNull();
        }
    }
}
