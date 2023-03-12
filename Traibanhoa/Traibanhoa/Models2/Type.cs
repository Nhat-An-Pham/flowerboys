using System;
using System.Collections.Generic;

#nullable disable

namespace Traibanhoa.Models2
{
    public partial class Type
    {
        public Type()
        {
            Products = new HashSet<Product>();
        }

        public Guid TypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
