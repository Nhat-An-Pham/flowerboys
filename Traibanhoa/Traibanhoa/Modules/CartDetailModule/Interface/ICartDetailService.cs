using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Traibanhoa.Modules.CartDetailModule.Request;
using Traibanhoa.Modules.CartDetailModule.Response;

namespace Traibanhoa.Modules.CartDetailModule.Interface
{
    public interface ICartDetailService
    {
        public Task<ICollection<ItemInCart>> GetCartDetailsByCartId(Guid cartId);
        public Task<UpdatedItemInCart> UpdateQuantityItemInCart(UpdatedItemInCart updatedItemInCart);
        public Task DeleteItemInCart(Guid cartId, Guid itemId, bool type);
    }
}
