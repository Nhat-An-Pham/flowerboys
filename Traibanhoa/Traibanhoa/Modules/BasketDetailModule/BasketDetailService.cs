using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Models.Models;
using Traibanhoa.Modules.BasketDetailModule.Request;
using Traibanhoa.Modules.BasketDetailModule.Interface;
using Models.Constant;
using Traibanhoa.Modules.TypeModule.Request;
using FluentValidation.Results;

namespace Traibanhoa.Modules.BasketDetailModule
{
    public class BasketDetailService : IBasketDetailService
    {
        private readonly IBasketDetailRepository _BasketDetailRepository;
        public BasketDetailService(IBasketDetailRepository BasketDetailRepository)
        {
            _BasketDetailRepository = BasketDetailRepository;
        }

        public async Task<ICollection<BasketDetail>> GetAll()
        {
            return await _BasketDetailRepository.GetAll(options: o => o.OrderByDescending(x => x.Quantity != 0).ToList());
        }

        public Task<ICollection<BasketDetail>> GetBasketDetailsBy(
                Expression<Func<BasketDetail,
                bool>> filter = null,
                Func<IQueryable<BasketDetail>,
                ICollection<BasketDetail>> options = null,
                string includeProperties = null)
        {
            return _BasketDetailRepository.GetBasketDetailsBy(filter);
        }


        public async Task<Guid?> AddNewBasketDetail(CreateBasketDetailRequest BasketDetailRequest)
        {
            ValidationResult result = new CreateBasketDetailRequestValidator().Validate(BasketDetailRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            var newBasketDetail = new BasketDetail();

            newBasketDetail.BasketId= BasketDetailRequest.BasketId;
            newBasketDetail.ProductId = BasketDetailRequest.ProductId;
            newBasketDetail.Quantity = BasketDetailRequest.Quantity;

            await _BasketDetailRepository.AddAsync(newBasketDetail);
            return newBasketDetail.BasketId;
        }

        public async Task UpdateBasketDetail(UpdateBasketDetailRequest BasketDetailRequest)
        {
            try
            {
                var BasketDetailUpdate = GetBasketDetailByID(BasketDetailRequest.BasketId).Result;

                if (BasketDetailUpdate == null)
                {
                    throw new Exception(ErrorMessage.BasketError.BASKET_NOT_FOUND);
                }

                ValidationResult result = new UpdateBasketDetailRequestValidator().Validate(BasketDetailRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                BasketDetailUpdate.BasketId = BasketDetailRequest.BasketId;
                BasketDetailUpdate.ProductId = BasketDetailRequest.ProductId;
                BasketDetailUpdate.Quantity = BasketDetailRequest.Quantity;

                await _BasketDetailRepository.UpdateAsync(BasketDetailUpdate);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteBasketDetail(Guid? basketDetailDeleteID)
        {
            try
            {
                if (basketDetailDeleteID == null)
                {
                    throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
                }

                BasketDetail basketDetailDelete = _BasketDetailRepository.GetFirstOrDefaultAsync(x => x.ProductId == basketDetailDeleteID && x.Quantity != 0).Result;

                if (basketDetailDelete == null)
                {
                    throw new Exception(ErrorMessage.ProductError.PRODUCT_NOT_FOUND);
                }

                basketDetailDelete.Quantity = 0;
                await _BasketDetailRepository.UpdateAsync(basketDetailDelete);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at delete type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<BasketDetail> GetBasketDetailByID(Guid? id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var basketDetail = await _BasketDetailRepository.GetFirstOrDefaultAsync(x => x.BasketId == id);
            if (basketDetail == null)
            {
                throw new Exception(ErrorMessage.BasketError.BASKET_NOT_FOUND);
            }
            return basketDetail;
        }
    }
}
