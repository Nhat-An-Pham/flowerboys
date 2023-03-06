using System;

namespace Traibanhoa.Modules.CartDetailModule.Request
{
    public class DeletedItemInCart
    {
        public Guid CartId { get; set; }
        public Guid ItemId { get; set; }
        public bool TypeItem { get; set; }
    }
}
