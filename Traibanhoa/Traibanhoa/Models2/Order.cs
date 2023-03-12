using System;
using System.Collections.Generic;

#nullable disable

namespace Traibanhoa.Models2
{
    public partial class Order
    {
        public Order()
        {
            OrderBasketDetails = new HashSet<OrderBasketDetail>();
            OrderProductDetails = new HashSet<OrderProductDetail>();
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
        public Guid? ConfirmBy { get; set; }

        public virtual User ConfirmByNavigation { get; set; }
        public virtual Customer OrderByNavigation { get; set; }
        public virtual ICollection<OrderBasketDetail> OrderBasketDetails { get; set; }
        public virtual ICollection<OrderProductDetail> OrderProductDetails { get; set; }
    }
}
