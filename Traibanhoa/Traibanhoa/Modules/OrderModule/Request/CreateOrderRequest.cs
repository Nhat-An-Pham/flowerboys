using FluentValidation;
using System;
using Traibanhoa.Modules.TypeModule.Request;

namespace Traibanhoa.Modules.OrderModule.Request
{
    public class CreateOrderRequest
    {
        public string ShippedAddress { get; set; }
        public string Phonenumber { get; set; }
        public string Email { get; set; }
        public decimal? TotalPrice { get; set; }
        public Guid? OrderBy { get; set; }
        public bool? IsRequest { get; set; }
        public int? PaymentMethod { get; set; }
    }
}
