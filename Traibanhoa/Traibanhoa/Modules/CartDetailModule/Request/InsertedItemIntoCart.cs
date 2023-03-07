using System;

namespace Traibanhoa.Modules.CartDetailModule.Request
{
    public class InsertedItemIntoCart
    {
        public Guid CustomerId { get; set; }
        public Guid ItemId { get; set; }
        public bool IsBasket { get; set; }
    }
}
