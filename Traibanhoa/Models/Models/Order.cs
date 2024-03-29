﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Transactions = new HashSet<Transaction>();
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

        public virtual Customer OrderByNavigation { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
