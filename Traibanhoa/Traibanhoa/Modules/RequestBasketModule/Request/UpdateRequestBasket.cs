using System.Collections.Generic;
using System;
using Traibanhoa.Modules.BasketModule.Request;

namespace Traibanhoa.Modules.RequestBasketModule.Request
{
    public class UpdateRequestBasket
    {
        public Guid RequestBasketId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public decimal? EstimatePrice { get; set; }
        public int? RequestStatus { get; set; }
        public Guid? ConfirmBy { get; set; }
        public List<BasketDetailRequest> BasketDetailRequests { get; set; }
    }
}
