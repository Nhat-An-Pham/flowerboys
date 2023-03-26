using Models.Models;
using System;
using System.Collections.Generic;

namespace Traibanhoa.Modules.OrderModule.Response
{
    public class OrderResponse
    {

        public OrderResponse()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public Guid OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public string ShippedAddress { get; set; }
        public string Phonenumber { get; set; }
        public string Email { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? OrderStatus { get; set; }
        public Guid? OrderBy { get; set; }
        public bool? IsRequest { get; set; }
        public int? PaymentMethod { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}

