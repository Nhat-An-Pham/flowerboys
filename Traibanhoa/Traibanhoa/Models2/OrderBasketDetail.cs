using System;
using System.Collections.Generic;

#nullable disable

namespace Traibanhoa.Models2
{
    public partial class OrderBasketDetail
    {
        public Guid OrderId { get; set; }
        public Guid BasketId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public bool? IsRequest { get; set; }

        public virtual Basket Basket { get; set; }
        public virtual RequestBasket BasketNavigation { get; set; }
        public virtual Order Order { get; set; }
    }
}
