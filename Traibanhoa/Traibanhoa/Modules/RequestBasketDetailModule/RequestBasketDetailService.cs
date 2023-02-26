using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Traibanhoa.Modules.RequestBasketDetailModule.Interface;
using Models.Models;
using Traibanhoa.Modules.RequestBasketDetailModule.Request;
using Models.Constant;
using Traibanhoa.Modules.TypeModule.Request;
using FluentValidation.Results;
using FluentValidation;

namespace Traibanhoa.Modules.RequestBasketDetailModule
{
    public class RequestBasketDetailService : IRequestBasketDetailService
    {
        private readonly IRequestBasketDetailRepository _RequestBasketDetailRepository;
        public RequestBasketDetailService(IRequestBasketDetailRepository RequestBasketDetailRepository)
        {
            _RequestBasketDetailRepository = RequestBasketDetailRepository;
        }

        public async Task<ICollection<RequestBasketDetail>> GetAll()
        {
            return await _RequestBasketDetailRepository.GetAll(options: o => o.OrderByDescending(x => x.Quantity != 0).ToList());
        }

        public Task<ICollection<RequestBasketDetail>> GetRequestBasketDetailsBy(
                Expression<Func<RequestBasketDetail,
                bool>> filter = null,
                Func<IQueryable<RequestBasketDetail>,
                ICollection<RequestBasketDetail>> options = null,
                string includeProperties = null)
        {
            return _RequestBasketDetailRepository.GetRequestBasketDetailsBy(filter);
        }


        public async Task<Guid?> AddNewRequestBasketDetail(CreateRequestBasketDetailRequest RequestBasketDetailRequest)
        {
            var newRequestBasketDetail = new RequestBasketDetail();

            ValidationResult result = new CreateRequestBasketDetailRequestValidator().Validate(RequestBasketDetailRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            newRequestBasketDetail.RequestBasketId = RequestBasketDetailRequest.RequestBasketId;
            newRequestBasketDetail.ProductId = RequestBasketDetailRequest.ProductId;
            newRequestBasketDetail.Quantity = RequestBasketDetailRequest.Quantity;


            await _RequestBasketDetailRepository.AddAsync(newRequestBasketDetail);
            return newRequestBasketDetail.RequestBasketId;
        }

        public async Task UpdateRequestBasketDetail(UpdateRequestBasketDetailRequest RequestBasketDetailRequest)
        {
            try
            {
                var RequestBasketDetailUpdate = GetRequestBasketDetailByID(RequestBasketDetailRequest.RequestBasketId).Result;
                
                if (RequestBasketDetailUpdate == null)
                {
                    throw new Exception(ErrorMessage.RequestError.REQUEST_NOT_FOUND);
                }

                ValidationResult result = new UpdateRequestBasketRequestDetailValidator().Validate(RequestBasketDetailRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                RequestBasketDetailUpdate.ProductId = RequestBasketDetailRequest.ProductId;
                RequestBasketDetailUpdate.Quantity = RequestBasketDetailRequest.Quantity;

                await _RequestBasketDetailRepository.UpdateAsync(RequestBasketDetailUpdate);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteRequestBasketDetail(Guid? requestBasketDetailDeleteID)
        {
            try
            {
                if (requestBasketDetailDeleteID == null)
                {
                    throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
                }

                RequestBasketDetail requestBasketDetailDelete = _RequestBasketDetailRepository.GetFirstOrDefaultAsync(x => x.ProductId == requestBasketDetailDeleteID && x.Quantity != 0).Result;

                if (requestBasketDetailDelete == null)
                {
                    throw new Exception(ErrorMessage.ProductError.PRODUCT_NOT_FOUND);
                }

                requestBasketDetailDelete.Quantity = 0;
                await _RequestBasketDetailRepository.UpdateAsync(requestBasketDetailDelete);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at delete type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<RequestBasketDetail> GetRequestBasketDetailByID(Guid? id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var requestBasketDetail = await _RequestBasketDetailRepository.GetFirstOrDefaultAsync(x => x.RequestBasketId == id);
            if (requestBasketDetail == null)
            {
                throw new Exception(ErrorMessage.RequestError.REQUEST_NOT_FOUND);
            }
            return requestBasketDetail;
        }

        //public async Task<ICollection<TypeDropdownResponse>> GetTypeDropdown()
        //{
        //    var result = await _typeRepository.GetTypesBy(x => x.Status == true);
        //    return result.Select(x => new TypeDropdownResponse
        //    {
        //        TypeId = x.TypeId,
        //        TypeName = x.Name
        //    }).ToList();
        //}
    }
}
