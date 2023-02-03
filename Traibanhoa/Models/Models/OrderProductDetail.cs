using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class OrderProductDetail
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
