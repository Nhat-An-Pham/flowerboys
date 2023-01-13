using System;
using System.Collections.Generic;

#nullable disable

namespace Traibanhoa.Models
{
    public partial class RequestBasketDetail
    {
        public Guid RequestBasketId { get; set; }
        public Guid ProductId { get; set; }
        public int? Quantity { get; set; }

        public virtual Product Product { get; set; }
        public virtual RequestBasket RequestBasket { get; set; }
    }
}
