using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class Cart
    {
        public Guid CartId { get; set; }
        public Guid? CustomerId { get; set; }
        public int? QuantityOfItem { get; set; }
    }
}
