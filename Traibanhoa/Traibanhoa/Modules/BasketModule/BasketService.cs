using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Traibanhoa.Modules.BasketModule.Interface;
using Models.Models;
using Traibanhoa.Modules.BasketModule.Request;
using Models.Constant;
using Traibanhoa.Modules.BasketModule.Response;
using Models.Enum;
using Traibanhoa.Modules.BasketDetailModule.Interface;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Text;
using Traibanhoa.Modules.BasketSubCateModule.Interface;

namespace Traibanhoa.Modules.BasketModule
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _BasketRepository;
        private readonly IBasketDetailRepository _basketDetailRepository;
        private readonly IBasketSubCateRepository _basketSubCateRepository;

        public BasketService(IBasketRepository BasketRepository, IBasketDetailRepository basketDetailRepository, IBasketSubCateRepository basketSubCateRepository)
        {
            _BasketRepository = BasketRepository;
            _basketDetailRepository = basketDetailRepository;
            _basketSubCateRepository = basketSubCateRepository;
        }

        public async Task<ICollection<Basket>> GetAll()
        {
            return await _BasketRepository.GetAll(options: o => o.OrderByDescending(x => x.UpdatedDate).ToList());
        }

        public async Task<ICollection<HomeNewBasketResponse>> GetNewBasketsForHome()
        {
            try
            {
                var result = _BasketRepository.GetBasketsBy(x => x.Status == (int?)BasketStatus.Active).Result.OrderByDescending(x => x.UpdatedDate).Take(6)
                    .Select(x => new HomeNewBasketResponse
                    {
                        BasketId = x.BasketId,
                        Title = x.Title,
                        Description = x.Description,
                        ImageUrl = x.ImageUrl
                    }).ToList();

                if (result.Count() == 0)
                {
                    throw new Exception(ErrorMessage.CommonError.LIST_IS_NULL);
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at GetNewBasketsForHome:" + ex.Message);
                throw;
            }
        }

        public List<ProductBasketDetail> GetProductBasketDetails(Guid basketId, ICollection<BasketDetail> basketDetails)
        {
            return _basketDetailRepository.GetBasketDetailsBy(x => x.BasketId == basketId, includeProperties: "Product").Result.Select(x => new ProductBasketDetail
            {
                ProductId = x.ProductId,
                ProductName = x.Product.Name,
                Quantity = (int)x.Quantity
            }).ToList();
        }

        public async Task<ICollection<DetailHomeViewBasketResponse>> GetMostViewBaskets()
        {
            try
            {
                var baskets = _BasketRepository.GetBasketsBy(x => x.Status == (int?)BasketStatus.Active).Result.OrderByDescending(x => x.View).Take(12).ToList();
                var basketsDetails = await _basketDetailRepository.GetAll(includeProperties: "Product");

                var result = baskets.Select(x => new DetailHomeViewBasketResponse
                {
                    BasketId = x.BasketId,
                    Title = x.Title,
                    Description = x.Description,
                    ImageUrl = x.ImageUrl,
                    BasketPrice = (decimal)x.BasketPrice,
                    View = (int)x.View,
                    ListProduct = GetProductBasketDetails(x.BasketId, basketsDetails)
                }).ToList();

                if (result.Count() == 0)
                {
                    throw new Exception(ErrorMessage.CommonError.LIST_IS_NULL);
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at GetMostViewBaskets:" + ex.Message);
                throw;
            }
        }

        public async Task<ICollection<DetailHomeViewBasketResponse>> GetBasketsByPrice()
        {
            try
            {
                var baskets = _BasketRepository.GetBasketsBy(x => x.Status == (int?)BasketStatus.Active && x.BasketPrice >= 200000 && x.BasketPrice <= 400000).Result
                    .OrderByDescending(x => x.UpdatedDate).Take(12).ToList();
                var basketsDetails = await _basketDetailRepository.GetAll(includeProperties: "Product");

                var result = baskets.Select(x => new DetailHomeViewBasketResponse
                {
                    BasketId = x.BasketId,
                    Title = x.Title,
                    Description = x.Description,
                    ImageUrl = x.ImageUrl,
                    BasketPrice = (decimal)x.BasketPrice,
                    View = (int)x.View,
                    ListProduct = GetProductBasketDetails(x.BasketId, basketsDetails)
                }).ToList();

                if (result.Count() == 0)
                {
                    throw new Exception(ErrorMessage.CommonError.LIST_IS_NULL);
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at GetBasketsByPrice:" + ex.Message);
                throw;
            }
        }

        public Task<ICollection<Basket>> GetBasketsBy(
                Expression<Func<Basket,
                bool>> filter = null,
                Func<IQueryable<Basket>,
                ICollection<Basket>> options = null,
                string includeProperties = null)
        {
            return _BasketRepository.GetBasketsBy(filter);
        }

        public async Task<Guid> AddNewBasket()
        {
            try
            {
                // add an empty basket
                Basket basket = new Basket()
                {
                    BasketId = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    View = 0,
                    Status = (int?)BasketStatus.Draft
                };
                await _BasketRepository.AddAsync(basket);

                return basket.BasketId;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at AddNewBasket: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateBasket(UpdateBasketRequest request)
        { return; }

        public async Task DeleteBasket(Guid? basketDeleteID)
        {
            try
            {
                if (basketDeleteID == null)
                {
                    throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
                }

                Basket basketDelete = _BasketRepository.GetFirstOrDefaultAsync(x => x.BasketId == basketDeleteID && x.Status == 0).Result;

                if (basketDelete == null)
                {
                    throw new Exception(ErrorMessage.BasketError.BASKET_NOT_FOUND);
                }

                basketDelete.Status = (int?)BasketStatus.Deactive;
                await _BasketRepository.UpdateAsync(basketDelete);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at delete type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Basket> GetBasketByID(Guid? id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var basket = await _BasketRepository.GetFirstOrDefaultAsync(x => x.BasketId == id);
            if (basket == null)
            {
                throw new Exception(ErrorMessage.BasketError.BASKET_NOT_FOUND);
            }
            return basket;
        }

        public async Task<ICollection<SearchBasketResponse>> GetBasketByName(String name)
        {
            var Baskets = await _BasketRepository.GetBasketsBy(x => x.Status == (int)BasketStatus.Active);
            var basketResponse = Baskets.Where(x => ConvertToUnSign(x.Title)
                .Contains(ConvertToUnSign(name), StringComparison.CurrentCultureIgnoreCase) || x.Title.Contains(name, StringComparison.CurrentCultureIgnoreCase))
                .ToList()
                .Select(x => new SearchBasketResponse
                {
                    BasketName = x.Title,
                    BasketId = x.BasketId
                }
                )
                .ToList();

            return basketResponse;
        }

        private string ConvertToUnSign(string input)
        {
            input = input.Trim();
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }
            return str2;
        }
    }
}
