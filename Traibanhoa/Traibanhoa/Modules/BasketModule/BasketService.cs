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

        public async Task<ICollection<GetBasketResponse>> GetAll()
        {
            var result = _BasketRepository.GetAll(options: o => o.OrderByDescending(x => x.UpdatedDate).ToList()).Result.Select(x => new GetBasketResponse
            {
                BasketId = x.BasketId,
                Title = x.Title,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                BasketPrice = (decimal)x.BasketPrice,
                View = (int)x.View,
                Status = x.Status,
                ListProduct = GetProductBasketDetails(x.BasketId)
            }).ToList();
            if (result.Count() == 0)
            {
                throw new Exception(ErrorMessage.CommonError.LIST_IS_NULL);
            }
            return result;
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

        public List<ProductBasketDetail> GetProductBasketDetails(Guid basketId)
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

                var result = baskets.Select(x => new DetailHomeViewBasketResponse
                {
                    BasketId = x.BasketId,
                    Title = x.Title,
                    Description = x.Description,
                    ImageUrl = x.ImageUrl,
                    BasketPrice = (decimal)x.BasketPrice,
                    View = (int)x.View,
                    ListProduct = GetProductBasketDetails(x.BasketId)
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

                var result = baskets.Select(x => new DetailHomeViewBasketResponse
                {
                    BasketId = x.BasketId,
                    Title = x.Title,
                    Description = x.Description,
                    ImageUrl = x.ImageUrl,
                    BasketPrice = (decimal)x.BasketPrice,
                    View = (int)x.View,
                    ListProduct = GetProductBasketDetails(x.BasketId)
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
        {
            try
            {
                #region validation
                // sub cates exceed limit
                if (request.BasketSubCates.Count > 5)
                    throw new Exception(ErrorMessage.BasketError.BASKET_SUBCATES_LIMIT);

                // basket not existed
                var basket = _BasketRepository
                    .GetBasketsBy(b => b.BasketId.Equals(request.BasketId),
                        options: (l) => l.AsNoTracking().ToList())
                    .Result
                    .FirstOrDefault();

                if (basket == null)
                    throw new Exception(ErrorMessage.BasketError.BASKET_NOT_FOUND);
                #endregion

                #region update recipe and recipe details
                // get products of basket
                var basketDetails = await _basketDetailRepository
                    .GetBasketDetailsBy(r => r.BasketId.Equals(basket.BasketId),
                        options: (l) => l.AsNoTracking().ToList());

                // check if exist then update, else add
                var joinBasketDetail = request.BasketDetailRequests
                    .Join(basketDetails, l => l.ProductId, r => r.ProductId,
                    (l, r) => l).ToList();

                foreach (var r in request.BasketDetailRequests)
                {
                    if (!joinBasketDetail.Contains(r))
                    {
                        var newBasketDetail = new BasketDetail
                        {
                            ProductId = r.ProductId,
                            Quantity = r.Quantity,
                            BasketId = basket.BasketId
                        };
                        await _basketDetailRepository.AddAsync(newBasketDetail);
                    }
                }
                // check if leftover then remove
                foreach (var rd in basketDetails)
                {
                    var r = joinBasketDetail.Find(r => r.ProductId.Equals(rd.ProductId));
                    if (r == null)
                        await _basketDetailRepository.RemoveAsync(rd);
                    else
                    {
                        rd.Quantity = r.Quantity;
                        await _basketDetailRepository.UpdateAsync(rd);
                    }
                }
                #endregion

                #region update basket and subcates
                // get sub cates of blog 
                var subCates = await _basketSubCateRepository
                    .GetBasketSubCatesBy(b => b.BasketId.Equals(request.BasketId),
                        options: (l) => l.AsNoTracking().ToList());

                // check if not exist then add
                var joinSubCate = request.BasketSubCates
                    .Join(subCates, l => l.SubCateId, r => r.SubCateId,
                    (l, r) => l).ToList();

                foreach (var b in request.BasketSubCates)
                {
                    if (!joinSubCate.Contains(b))
                    {
                        var newBasketSubCate = new BasketSubCate
                        {
                            BasketId = basket.BasketId,
                            CreatedDate = DateTime.Now,
                            Status = true,
                            SubCateId = b.SubCateId
                        };
                        await _basketSubCateRepository.AddAsync(newBasketSubCate);
                    }
                }
                // check if leftover then remove
                foreach (var s in subCates)
                {
                    if (!joinSubCate.Select(j => j.SubCateId).ToList().Contains(s.SubCateId))
                    {
                        await _basketSubCateRepository.RemoveAsync(s);
                    }
                }

                // update basket
                basket.UpdatedDate = DateTime.Now;
                basket.Title = request.Title;
                basket.Description = request.Description;
                basket.ImageUrl = request.ImageUrl;
                basket.BasketPrice = request.BasketPrice;
                basket.Status = request.Status;
                await _BasketRepository.UpdateAsync(basket);
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at UpdateBasket: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteBasket(Guid? basketDeleteID)
        {
            try
            {
                if (basketDeleteID == null)
                {
                    throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
                }

                Basket basketDelete = _BasketRepository.GetFirstOrDefaultAsync(x => x.BasketId == basketDeleteID).Result;

                if (basketDelete == null)
                {
                    throw new Exception(ErrorMessage.BasketError.BASKET_NOT_FOUND);
                }

                var listBasketSubCate = await _basketSubCateRepository.GetBasketSubCatesBy(x => x.BasketId == basketDelete.BasketId);

                if (basketDelete.Status == (int?)BasketStatus.Active)
                {
                    foreach (var item in listBasketSubCate)
                    {
                        item.Status = false;
                    }
                    await _basketSubCateRepository.UpdateRangeAsync(listBasketSubCate);

                    basketDelete.Status = (int?)BasketStatus.Deactive;
                    await _BasketRepository.UpdateAsync(basketDelete);

                } else if (basketDelete.Status == (int?)BasketStatus.Draft) {
                    await _basketSubCateRepository.RemoveRangeAsync(listBasketSubCate);
                    var listBasketDetail = await _basketDetailRepository.GetBasketDetailsBy(x => x.BasketId == basketDelete.BasketId);

                    await _basketDetailRepository.RemoveRangeAsync(listBasketDetail);

                    basketDelete.Status = (int?)BasketStatus.Deactive;
                    await _BasketRepository.RemoveAsync(basketDelete);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at delete basket: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task RestoreBasket(Guid? basketRestoreID)
        {
            try
            {
                if (basketRestoreID == null)
                {
                    throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
                }

                Basket basketRestore = _BasketRepository.GetFirstOrDefaultAsync(x => x.BasketId == basketRestoreID && x.Status == (int?) BasketStatus.Deactive).Result;

                if (basketRestore == null)
                {
                    throw new Exception(ErrorMessage.BasketError.BASKET_NOT_FOUND);
                }

                var listBasketSubCate = await _basketSubCateRepository.GetBasketSubCatesBy(x => x.BasketId == basketRestore.BasketId && x.Status == false);
                foreach (var item in listBasketSubCate)
                {
                    item.Status = true;
                }

                basketRestore.Status = (int?)BasketStatus.Active;
                await _basketSubCateRepository.UpdateRangeAsync(listBasketSubCate);
                await _BasketRepository.UpdateAsync(basketRestore);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at restore basket: " + ex.Message);
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
