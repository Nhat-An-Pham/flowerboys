using System;
using System.Collections.Generic;
using Traibanhoa.Modules.BasketModule.Response;

namespace Traibanhoa.Modules.RequestBasketModule.Response
{
    public class GetRequestBasketResponse
    {
        public Guid RequestBasketId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public int? Status { get; set; }
        public decimal? EstimatePrice { get; set; }
        public string CreateBy { get; set; }
        public string ConfirmBy { get; set; }
        public List<ProductBasketDetail> ListProduct { get; set; }
    }
}
