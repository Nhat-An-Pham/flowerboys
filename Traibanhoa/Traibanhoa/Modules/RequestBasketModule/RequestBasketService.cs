using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Traibanhoa.Modules.RequestBasketModule.Interface;
using Models.Models;
using Traibanhoa.Modules.RequestBasketModule.Request;

namespace Traibanhoa.Modules.RequestBasketModule
{
    public class RequestBasketDetailService : IRequestBasketService
    {
        private readonly IRequestBasketRepository _RequestBasketRepository;
        public RequestBasketDetailService(IRequestBasketRepository RequestBasketRepository)
        {
            _RequestBasketRepository = RequestBasketRepository;
        }

        public async Task<ICollection<RequestBasket>> GetAll()
        {
            return await _RequestBasketRepository.GetAll();
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


        public async Task<Boolean> AddNewRequestBasket(CreateRequestBasketRequest RequestBasketRequest)
        {
            var newRequestBasket = new RequestBasket();

            if (_RequestBasketRepository.GetFirstOrDefaultAsync(x => x.RequestBasketId == RequestBasketRequest.RequestBasketId) == null)
            {
                return false;
            }
            newRequestBasket.Title = RequestBasketRequest.Title;
            newRequestBasket.ImageUrl = RequestBasketRequest.ImageUrl;
            newRequestBasket.CreatedDate = DateTime.Now;
            newRequestBasket.RequestStatus = RequestBasketRequest.RequestStatus;
            newRequestBasket.CreateBy = RequestBasketRequest.CreateBy;
            newRequestBasket.ConfirmBy = RequestBasketRequest.ConfirmBy;

            await _RequestBasketRepository.AddAsync(newRequestBasket);
            return true;
        }

        public async Task<Boolean> UpdateRequestBasket(UpdateRequestBasketRequest RequestBasketRequest)
        {
            var RequestBasketUpdate = GetRequestBasketByID(RequestBasketRequest.RequestBasketId).Result;

            RequestBasketUpdate.Title = RequestBasketRequest.Title;
            RequestBasketUpdate.ImageUrl = RequestBasketRequest.ImageUrl;
            RequestBasketUpdate.RequestStatus = RequestBasketRequest.RequestStatus;
            RequestBasketUpdate.CreateBy = RequestBasketRequest.CreateBy;
            RequestBasketUpdate.ConfirmBy = RequestBasketRequest.ConfirmBy;

            await _RequestBasketRepository.UpdateAsync(RequestBasketUpdate);
            return true;
        }

        //public async Task<Boolean> DeleteRequestBasket(RequestBasket RequestBasketDelete)
        //{
        //    RequestBasketDelete.Status = 0;
        //    await _RequestBasketRepository.UpdateAsync(RequestBasketDelete);
        //    return true;
        //}

        public async Task<RequestBasket> GetRequestBasketByID(Guid? id)
        {
            return await _RequestBasketRepository.GetFirstOrDefaultAsync(x => x.RequestBasketId == id);
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
