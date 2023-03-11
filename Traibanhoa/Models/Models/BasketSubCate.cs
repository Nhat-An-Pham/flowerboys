using Models.Models;
using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class BasketSubCate
    {
        public Guid BasketId { get; set; }
        public Guid SubCateId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? Status { get; set; }

        public virtual Basket Basket { get; set; }
        public virtual SubCategory SubCate { get; set; }
    }
}
