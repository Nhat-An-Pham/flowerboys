using System;
using System.Collections.Generic;

#nullable disable

namespace Traibanhoa.Models2
{
    public partial class SubCategory
    {
        public SubCategory()
        {
            BasketSubCates = new HashSet<BasketSubCate>();
        }

        public Guid SubCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? Status { get; set; }
        public Guid? CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<BasketSubCate> BasketSubCates { get; set; }
    }
}
