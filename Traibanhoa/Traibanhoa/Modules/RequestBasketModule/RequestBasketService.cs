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

namespace Traibanhoa.Modules.RequestBasketModule
{
    public class RequestBasketService : IRequestBasketService
    {
        private readonly IRequestBasketRepository _RequestBasketRepository;
        public RequestBasketService(IRequestBasketRepository RequestBasketRepository)
        {
            _RequestBasketRepository = RequestBasketRepository;
        }

        public async Task<ICollection<RequestBasket>> GetAll()
        {
            return await _RequestBasketRepository.GetAll(options: o => o.OrderByDescending(x => x.CreatedDate).ToList());
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


        public async Task<Guid?> AddNewRequestBasket(CreateRequestBasketRequest RequestBasketRequest)
        {
            var newRequestBasket = new RequestBasket();

            ValidationResult result = new CreateRequestBasketRequestValidator().Validate(RequestBasketRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            newRequestBasket.RequestBasketId = Guid.NewGuid();
            newRequestBasket.Title = RequestBasketRequest.Title;
            newRequestBasket.ImageUrl = RequestBasketRequest.ImageUrl;
            newRequestBasket.CreatedDate = DateTime.Now;
            newRequestBasket.RequestStatus = RequestBasketRequest.RequestStatus;
            newRequestBasket.CreateBy = RequestBasketRequest.CreateBy;
            newRequestBasket.ConfirmBy = RequestBasketRequest.ConfirmBy;

            await _RequestBasketRepository.AddAsync(newRequestBasket);
            return newRequestBasket.RequestBasketId;
        }

        public async Task UpdateRequestBasket(UpdateRequestBasketRequest RequestBasketRequest)
        {
            try
            {
                var RequestBasketUpdate = GetRequestBasketByID(RequestBasketRequest.RequestBasketId).Result;

                if (RequestBasketUpdate == null)
                {
                    throw new Exception(ErrorMessage.RequestError.REQUEST_NOT_FOUND);
                }

                ValidationResult result = new UpdateRequestBasketRequestValidator().Validate(RequestBasketRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                RequestBasketUpdate.Title = RequestBasketRequest.Title;
                RequestBasketUpdate.ImageUrl = RequestBasketRequest.ImageUrl;
                RequestBasketUpdate.RequestStatus = RequestBasketRequest.RequestStatus;
                RequestBasketUpdate.CreateBy = RequestBasketRequest.CreateBy;
                RequestBasketUpdate.ConfirmBy = RequestBasketRequest.ConfirmBy;

                await _RequestBasketRepository.UpdateAsync(RequestBasketUpdate);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        //public async Task<Boolean> DeleteRequestBasket(RequestBasket RequestBasketDelete)
        //{
        //    RequestBasketDelete.Status = 0;
        //    await _RequestBasketRepository.UpdateAsync(RequestBasketDelete);
        //    return true;
        //}

        public async Task<RequestBasket> GetRequestBasketByID(Guid? id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var requestBasket = await _RequestBasketRepository.GetFirstOrDefaultAsync(x => x.RequestBasketId == id);
            if (requestBasket == null)
            {
                throw new Exception(ErrorMessage.RequestError.REQUEST_NOT_FOUND);
            }
            return requestBasket;
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
