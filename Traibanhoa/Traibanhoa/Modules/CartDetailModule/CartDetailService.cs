using Models.Constant;
using Models.Enum;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Traibanhoa.Modules.BasketModule.Interface;
using Traibanhoa.Modules.CartDetailModule.Interface;
using Traibanhoa.Modules.CartDetailModule.Request;
using Traibanhoa.Modules.CartDetailModule.Response;
using Traibanhoa.Modules.CartModule.Interface;
using Traibanhoa.Modules.ProductModule.Interface;

namespace Traibanhoa.Modules.CartDetailModule
{
    public class CartDetailService : ICartDetailService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartDetailRepository _cartDetailRepository;
        private readonly IBasketRepository _basketRepository;
        private readonly IProductRepository _productRepository;

        public CartDetailService(ICartRepository cartRepository, ICartDetailRepository cartDetailRepository,
            IBasketRepository basketRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _cartDetailRepository = cartDetailRepository;
            _basketRepository = basketRepository;
            _productRepository = productRepository;
        }

        public Task<ICollection<CartDetail>> GetCartDetailsBy(
            Expression<Func<CartDetail, bool>> filter = null,
            Func<IQueryable<CartDetail>, ICollection<CartDetail>> options = null,
            string includeProperties = null)
        {
            return _cartDetailRepository.GetCartDetailsBy(filter);
        }

        private async Task<ICollection<ItemInCart>> ToResponse(List<CartDetail> list)
        {
            if (list != null)
            {
                var result = new List<ItemInCart>();

                ICollection<Basket> baskets = await _basketRepository.GetBasketsBy(b => b.Status == (int)BasketStatus.Active);
                ICollection<Product> products = await _productRepository.GetProductsBy(r => r.Status == true);
                var mappedProducts = list.Join(products, cd => cd.ItemId, i => i.ProductId,
                    (cd, i) => new ItemInCart()
                    {
                        ItemId = cd.ItemId,
                        Name = i.Name,
                        Image = i.Picture,
                        Quantity = cd.Quantity.Value,
                        UnitPrice = cd.UnitPrice.Value
                    }
                ).ToList();

                var mappedBaskets = list.Join(baskets, cd => cd.ItemId, i => i.BasketId,
                    (cd, i) => new ItemInCart()
                    {
                        ItemId = cd.ItemId,
                        Name = i.Title,
                        Image = i.ImageUrl,
                        Quantity = cd.Quantity.Value,
                        UnitPrice = cd.UnitPrice.Value
                    }
                ).ToList();

                if (mappedProducts != null && mappedProducts.Count > 0)
                {
                    result.AddRange(mappedProducts);
                }
                if (mappedBaskets != null && mappedBaskets.Count > 0)
                {
                    result.AddRange(mappedBaskets);
                }
                return result;
            }
            return null;
        }

        public async Task<ICollection<ItemInCart>> GetCartDetailsByCartId(Guid cartId)
        {
            ICollection<ItemInCart> result = null;
            try
            {
                var cartDetails = await _cartDetailRepository.GetCartDetailsBy(c => c.CartId == cartId);
                if (cartDetails == null || cartDetails.Count == 0)
                {
                    throw new Exception(ErrorMessage.CartError.CART_NOT_FOUND);
                }
                else
                {
                    result = await ToResponse(cartDetails.ToList());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at GetCartDetailsByCartId: " + ex.Message);
                throw new Exception(ex.Message);
            }
            return result;
        }

        public async Task<UpdatedItemInCart> UpdateQuantityItemInCart(UpdatedItemInCart updatedItemInCart)
        {
            try
            {
                if (updatedItemInCart == null || updatedItemInCart.Quantity < 0)
                {
                    throw new Exception(ErrorMessage.CartError.QUANTITY_NOT_VALID);
                }

                var item = _cartDetailRepository.GetFirstOrDefaultAsync(cd =>
                                                    cd.CartId == updatedItemInCart.CartId
                                                    && cd.ItemId == updatedItemInCart.ItemId).Result;

                if (item == null)
                {
                    throw new Exception(ErrorMessage.CartError.ITEM_NOT_FOUND);
                }
                else
                {
                    item.Quantity = updatedItemInCart.Quantity;
                    await _cartDetailRepository.UpdateAsync(item);
                    return updatedItemInCart;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at UpdateQuantityItemInCart: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteItemInCart(Guid cartId, Guid itemId, bool type)
        {
            try
            {
                var item = _cartDetailRepository.GetFirstOrDefaultAsync(
                                                    cd => cd.CartId == cartId
                                                    && cd.ItemId == itemId).Result;
                if (item == null)
                {
                    throw new Exception(ErrorMessage.CartError.ITEM_NOT_FOUND);
                }
                else
                {
                    var cart = await _cartRepository.GetFirstOrDefaultAsync(c => c.CartId == cartId);
                    if (cart == null)
                    {
                        throw new Exception(ErrorMessage.CartError.CART_NOT_FOUND);
                    }
                    --cart.QuantityOfItem;
                    await _cartDetailRepository.RemoveAsync(item);
                    await _cartRepository.UpdateAsync(cart);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at DeleteItemInCart: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
