﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class RequestBasket
    {
        public RequestBasket()
        {
            RequestBasketDetails = new HashSet<RequestBasketDetail>();
        }

        public Guid RequestBasketId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Phone { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? RequestStatus { get; set; }
        public decimal? EstimatePrice { get; set; }
        public Guid? CreateBy { get; set; }
        public Guid? ConfirmBy { get; set; }

        public virtual User ConfirmByNavigation { get; set; }
        public virtual Customer CreateByNavigation { get; set; }
        public virtual ICollection<RequestBasketDetail> RequestBasketDetails { get; set; }
    }
}
