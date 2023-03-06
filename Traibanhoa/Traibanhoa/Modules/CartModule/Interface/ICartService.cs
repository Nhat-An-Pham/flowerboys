using Models.Models;
using System;
using System.Threading.Tasks;
using Traibanhoa.Modules.CartDetailModule.Request;

namespace Traibanhoa.Modules.CartModule.Interface
{
    public interface ICartService
    {
        public Cart GetCartByCustomerId(Guid customerId);
        public Task<Cart> InsertNewItemIntoCart(InsertedItemIntoCart newItem);
    }
}
