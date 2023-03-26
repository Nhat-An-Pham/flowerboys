using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class OrderDetail
    {
        public Guid OrderDetailId { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? ProductId { get; set; }
        public int? Quantity { get; set; }
        public Guid? BasketId { get; set; }
        public decimal? Price { get; set; }
        public Guid? RequestBasketId { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
