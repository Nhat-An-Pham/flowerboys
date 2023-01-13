using System;
using System.Collections.Generic;

#nullable disable

namespace Traibanhoa.Models
{
    public partial class Product
    {
        public Product()
        {
            BasketDetails = new HashSet<BasketDetail>();
            OrderProductDetails = new HashSet<OrderProductDetail>();
            RequestBasketDetails = new HashSet<RequestBasketDetail>();
        }

        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? Status { get; set; }
        public decimal? Price { get; set; }
        public Guid? TypeId { get; set; }

        public virtual Type Type { get; set; }
        public virtual ICollection<BasketDetail> BasketDetails { get; set; }
        public virtual ICollection<OrderProductDetail> OrderProductDetails { get; set; }
        public virtual ICollection<RequestBasketDetail> RequestBasketDetails { get; set; }
    }
}
