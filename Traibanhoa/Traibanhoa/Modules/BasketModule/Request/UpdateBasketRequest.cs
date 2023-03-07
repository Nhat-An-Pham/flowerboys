using FluentValidation;
using System;
using System.Collections.Generic;

namespace Traibanhoa.Modules.BasketModule.Request
{
    public class UpdateBasketRequest
    {
        public Guid BasketId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal? BasketPrice { get; set; }
        public int Status { get; set; }
        public List<BasketDetailRequest> BasketDetailRequests { get; set; }
        public List<BasketSubCateRequest> BasketSubCates { get; set; }

    }

    public class BasketDetailRequest
    {
        public Guid ProductId { get; set; }
        public int? Quantity { get; set; }
    }

    public class BasketSubCateRequest
    {
        public Guid SubCateId { get; set; }
    }
}
