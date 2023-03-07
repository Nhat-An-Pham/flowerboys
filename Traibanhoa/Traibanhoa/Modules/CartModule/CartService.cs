using Models.Constant;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Traibanhoa.Modules.BasketModule.Interface;
using Traibanhoa.Modules.CartDetailModule.Interface;
using Traibanhoa.Modules.CartDetailModule.Request;
using Traibanhoa.Modules.CartModule.Interface;
using Traibanhoa.Modules.CustomerModule.Interface;
using Traibanhoa.Modules.ProductModule.Interface;

namespace Traibanhoa.Modules.CartModule
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartDetailRepository _cartDetailRepository;
        private readonly IBasketRepository _basketRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;

        public CartService(ICartRepository cartRepository, ICartDetailRepository cartDetailRepository,
           IBasketRepository basketRepository, IProductRepository productRepository,
            ICustomerRepository customerRepository)
        {
            _cartRepository = cartRepository;
            _cartDetailRepository = cartDetailRepository;
            _basketRepository = basketRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
        }

        public Cart GetCartByCustomerId(Guid customerId)
        {
            Cart result = null;
            try
            {
                CheckCustomerIsExisted(customerId);
                result = _cartRepository.GetFirstOrDefaultAsync(c => c.CustomerId == customerId).Result;
                if (result == null)
                {
                    throw new Exception(ErrorMessage.CartError.CART_NOT_FOUND);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at GetCartByCustomerId: " + ex.Message);
                throw new Exception(ex.Message);
            }
            return result;
        }

        public async Task<Cart> InsertNewItemIntoCart(InsertedItemIntoCart newItem)
        {
            Cart cart = null;
            try
            {
                CheckCustomerIsExisted(newItem.CustomerId);
                await CheckItemIsExisted(newItem.ItemId, newItem.IsBasket);
                cart = CheckCartExistedByCustomerId(newItem.CustomerId);

                // No exist cart with this customerId
                if (cart == null)
                {
                    // Init new cart for this customerId
                    cart = new Cart()
                    {
                        CartId = Guid.NewGuid(),
                        CustomerId = newItem.CustomerId,
                        QuantityOfItem = 1
                    };
                    await _cartRepository.AddAsync(cart);

                    // Insert new item into cart detail
                    await _cartDetailRepository.AddAsync(await InitCartDetail(cart.CartId, newItem.ItemId, newItem.IsBasket));
                }
                // existed cart with this customerid
                else
                {
                    ICollection<CartDetail> cartDetails = await _cartDetailRepository.GetCartDetailsBy(cd => cd.CartId == cart.CartId);
                    var cd = cartDetails.FirstOrDefault(x => x.ItemId == newItem.ItemId);
                    if (cd == null)
                    {
                        await _cartDetailRepository.AddAsync(await InitCartDetail(cart.CartId, newItem.ItemId, newItem.IsBasket));
                        ++cart.QuantityOfItem;
                        await _cartRepository.UpdateAsync(cart);
                    }
                    else
                    {
                        ++cd.Quantity;
                        await _cartDetailRepository.UpdateAsync(cd);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at InsertNewItemIntoCart: " + ex.Message);
                throw;
            }
            return cart;
        }

        private async Task CheckItemIsExisted(Guid itemId, bool isBasket)
        {
            try
            {
                if (isBasket)
                {
                    var item = await _basketRepository.GetByIdAsync(itemId);
                    if (item == null)
                    {
                        throw new Exception(ErrorMessage.CartError.ITEM_NOT_FOUND);
                    }
                }
                else
                {
                    var product = await _productRepository.GetByIdAsync(itemId);
                    if (product == null)
                    {
                        throw new Exception(ErrorMessage.CartError.ITEM_NOT_FOUND);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at CheckItemIsExisted: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private void CheckCustomerIsExisted(Guid customerId)
        {
            try
            {
                var customer = _customerRepository.GetFirstOrDefaultAsync(c => c.CustomerId == customerId).Result;
                if (customer == null)
                {
                    throw new Exception(ErrorMessage.CustomerError.CUSTOMER_NOT_FOUND);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at CheckCustomerIsExisted: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private Cart CheckCartExistedByCustomerId(Guid customerId)
        {
            try
            {
                return _cartRepository.GetFirstOrDefaultAsync(c => c.CustomerId == customerId).Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at CheckCartExistedByCustomerId: " + ex.Message);
                throw;
            }
        }

        public async Task<CartDetail> InitCartDetail(Guid cartId, Guid itemId, bool isBasket)
        {
            var cartDetail = new CartDetail()
            {
                CartId = cartId,
                ItemId = itemId,
                Quantity = 1
            };

            // Get price based on type item
            if (isBasket)
            {
                var basketItem = await _basketRepository.GetByIdAsync(itemId);
                cartDetail.UnitPrice = basketItem.BasketPrice;
            }
            else
            {
                var productItem = await _productRepository.GetByIdAsync(itemId);
                if (productItem != null) // product
                {
                    cartDetail.UnitPrice = productItem.Price;
                }
            }
            return cartDetail;
        }

    }
}
