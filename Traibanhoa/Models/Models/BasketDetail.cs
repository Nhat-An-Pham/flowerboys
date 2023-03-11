using Models.Models;
using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class BasketDetail
    {
        public Guid BasketId { get; set; }
        public Guid ProductId { get; set; }
        public int? Quantity { get; set; }

        public virtual Basket Basket { get; set; }
        public virtual Product Product { get; set; }
    }
}
