﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class Transaction
    {
        public Guid TransactionId { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? TransactionStatus { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? CustomerId { get; set; }

        public virtual Order Order { get; set; }
    }
}
