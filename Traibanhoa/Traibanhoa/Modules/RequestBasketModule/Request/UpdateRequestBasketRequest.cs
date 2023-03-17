using FluentValidation;
using System;
using System.Collections.Generic;

namespace Traibanhoa.Modules.RequestBasketModule.Request
{
    public class UpdateRequestBasketRequest
    {
        public Guid RequestBasketId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public decimal? EstimatePrice { get; set; }
        public int? RequestStatus { get; set; }
        public Guid? ConfirmBy { get; set; }
        public List<BasketDetailRequest> BasketDetailRequests { get; set; }
    }

    public class BasketDetailRequest
    {
        public Guid ProductId { get; set; }
        public int? Quantity { get; set; }
    }
}
