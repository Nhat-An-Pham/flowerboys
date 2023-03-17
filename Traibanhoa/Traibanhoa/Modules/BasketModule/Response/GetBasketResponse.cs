using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Traibanhoa.Modules.BasketModule.Response
{
    public class GetBasketResponse
    {
        public Guid BasketId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int View { get; set; }
        public int? Status { get; set; }
        public decimal BasketPrice { get; set; }
        public List<ProductBasketDetail> ListProduct { get; set; }
    }
}
