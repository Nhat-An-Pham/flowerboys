using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Traibanhoa.Modules.RequestBasketModule.Interface;
using Models.Models;
using Traibanhoa.Modules.RequestBasketModule.Request;
using Models.Constant;
using Traibanhoa.Modules.TypeModule.Request;
using FluentValidation.Results;
using FluentValidation;
using Models.Enum;
using System.Text.RegularExpressions;
using System.Text;
using Traibanhoa.Modules.BasketModule.Request;
using Traibanhoa.Modules.BasketModule.Response;
using Traibanhoa.Modules.RequestBasketDetailModule.Interface;
using Traibanhoa.Modules.RequestBasketModule.Response;
using Traibanhoa.Modules.UserModule.Interface;
using Microsoft.EntityFrameworkCore;

namespace Traibanhoa.Modules.RequestBasketModule
{
    public class RequestBasketService : IRequestBasketService
    {
        private readonly IRequestBasketRepository _RequestBasketRepository;
        private readonly IRequestBasketDetailRepository _requestBasketDetailRepository;
        public RequestBasketService(IRequestBasketRepository RequestBasketRepository, IRequestBasketDetailRepository requestBasketDetailRepository)
        {
            _RequestBasketRepository = RequestBasketRepository;
            _requestBasketDetailRepository = requestBasketDetailRepository;
        }

        public async Task<ICollection<GetRequestBasketResponse>> GetAll()
        {
            var result = _RequestBasketRepository.GetAll(options: o => o.OrderByDescending(x => x.CreatedDate).ToList(), includeProperties: "ConfirmByNavigation, CreateByNavigation").Result.Select(x => new GetRequestBasketResponse
            {
                RequestBasketId = x.RequestBasketId,
                Title = x.Title,
                ImageUrl = x.ImageUrl,
                Status = x.RequestStatus,
                EstimatePrice = x.EstimatePrice,
                ConfirmBy = x.ConfirmByNavigation.Name,
                CreateBy = x.CreateByNavigation.Name,
                ListProduct = GetProductRequestBasketDetails(x.RequestBasketId)
            }).ToList();
            if (result.Count() == 0)
            {
                throw new Exception(ErrorMessage.CommonError.LIST_IS_NULL);
            }
            return result;
        }

        public List<ProductBasketDetail> GetProductRequestBasketDetails(Guid requestBasketId)
        {
            return _requestBasketDetailRepository.GetRequestBasketDetailsBy(x => x.RequestBasketId == requestBasketId, includeProperties: "Product").Result.Select(x => new ProductBasketDetail
            {
                ProductId = x.ProductId,
                ProductName = x.Product.Name,
                Quantity = (int)x.Quantity
            }).ToList();
        }

        public Task<ICollection<RequestBasket>> GetRequestBasketsBy(
                Expression<Func<RequestBasket,
                bool>> filter = null,
                Func<IQueryable<RequestBasket>,
                ICollection<RequestBasket>> options = null,
                string includeProperties = null)
        {
            return _RequestBasketRepository.GetRequestBasketsBy(filter);
        }

        public async Task<Guid> AddNewRequestBasket(Guid? currentCustomerId)
        {
            try
            {
                // add an empty requestBasket
                RequestBasket requestBasket = new RequestBasket()
                {
                    RequestBasketId = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    RequestStatus = (int?)BasketStatus.Draft,
                    CreateBy = currentCustomerId
                };
                await _RequestBasketRepository.AddAsync(requestBasket);

                return requestBasket.RequestBasketId;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at AddNewRequestBasket: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateRequestBasket(UpdateRequestBasketRequest request)
        {
            try
            {
                #region validation

                // requestBasket not existed
                var requestBasket = _RequestBasketRepository
                    .GetRequestBasketsBy(b => b.RequestBasketId.Equals(request.RequestBasketId),
                        options: (l) => l.AsNoTracking().ToList())
                    .Result
                    .FirstOrDefault();

                if (requestBasket == null)
                    throw new Exception(ErrorMessage.RequestBasketError.REQUEST_BASKET_NOT_FOUND);
                #endregion

                #region update request basket and request basket details
                // get products of request basket
                var requestBasketDetails = await _requestBasketDetailRepository
                    .GetRequestBasketDetailsBy(r => r.RequestBasketId.Equals(requestBasket.RequestBasketId),
                        options: (l) => l.AsNoTracking().ToList());

                // check if exist then update, else add
                var joinRequestBasketDetail = request.BasketDetailRequests
                    .Join(requestBasketDetails, l => l.ProductId, r => r.ProductId,
                    (l, r) => l).ToList();

                foreach (var r in request.BasketDetailRequests)
                {
                    if (!joinRequestBasketDetail.Contains(r))
                    {
                        var newBasketDetail = new RequestBasketDetail
                        {
                            ProductId = r.ProductId,
                            Quantity = r.Quantity,
                            RequestBasketId = requestBasket.RequestBasketId
                        };
                        await _requestBasketDetailRepository.AddAsync(newBasketDetail);
                    }
                }
                // check if leftover then remove
                foreach (var rd in requestBasketDetails)
                {
                    var r = joinRequestBasketDetail.Find(r => r.ProductId.Equals(rd.ProductId));
                    if (r == null)
                        await _requestBasketDetailRepository.RemoveAsync(rd);
                    else
                    {
                        rd.Quantity = r.Quantity;
                        await _requestBasketDetailRepository.UpdateAsync(rd);
                    }
                }
                #endregion

                #region update basket

                // update request basket
                requestBasket.Title = request.Title;
                requestBasket.ImageUrl = request.ImageUrl;
                requestBasket.EstimatePrice = request.EstimatePrice;
                requestBasket.RequestStatus = request.RequestStatus;
                requestBasket.ConfirmBy = request.ConfirmBy;
                await _RequestBasketRepository.UpdateAsync(requestBasket);
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at UpdateBasket: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteRequestBasket(Guid? requestBasketDeleteID)
        {
            try
            {
                if (requestBasketDeleteID == null)
                {
                    throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
                }

                RequestBasket requestBasketDelete = _RequestBasketRepository.GetFirstOrDefaultAsync(x => x.RequestBasketId == requestBasketDeleteID).Result;

                if (requestBasketDelete == null)
                {
                    throw new Exception(ErrorMessage.BasketError.BASKET_NOT_FOUND);
                }
                var listBasketDetail = await _requestBasketDetailRepository.GetRequestBasketDetailsBy(x => x.RequestBasketId == requestBasketDelete.RequestBasketId);

                await _requestBasketDetailRepository.RemoveRangeAsync(listBasketDetail);

                await _RequestBasketRepository.RemoveAsync(requestBasketDelete);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at delete request basket: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<RequestBasket> GetRequestBasketByID(Guid? id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var requestBasket = await _RequestBasketRepository.GetFirstOrDefaultAsync(x => x.RequestBasketId == id);
            if (requestBasket == null)
            {
                throw new Exception(ErrorMessage.RequestBasketError.REQUEST_BASKET_NOT_FOUND);
            }
            return requestBasket;
        }

        public async Task<RequestBasket> GetRequestBasketByAuthorID(Guid? id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var requestBasket = await _RequestBasketRepository.GetFirstOrDefaultAsync(x => x.CreateBy == id);
            if (requestBasket == null)
            {
                throw new Exception(ErrorMessage.RequestBasketError.REQUEST_BASKET_NOT_FOUND);
            }
            return requestBasket;
        }
        public async Task<RequestBasket> GetRequestBasketByConfirmerID(Guid? id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var requestBasket = await _RequestBasketRepository.GetFirstOrDefaultAsync(x => x.ConfirmBy == id);
            if (requestBasket == null)
            {
                throw new Exception(ErrorMessage.RequestBasketError.REQUEST_BASKET_NOT_FOUND);
            }
            return requestBasket;
        }
    }
}
