using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Traibanhoa.Modules.RequestBasketDetailModule.Interface;
using Models.Models;
using Traibanhoa.Modules.RequestBasketDetailModule.Request;

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
            return await _RequestBasketDetailRepository.GetAll();
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


        public async Task<Boolean> AddNewRequestBasketDetail(CreateRequestBasketDetailRequest RequestBasketDetailRequest)
        {
            var newRequestBasketDetail = new RequestBasketDetail();

            if (_RequestBasketDetailRepository.GetFirstOrDefaultAsync(x => x.RequestBasketId == RequestBasketDetailRequest.RequestBasketId) == null)
            {
                return false;
            }
            newRequestBasketDetail.RequestBasketId = RequestBasketDetailRequest.RequestBasketId;
            newRequestBasketDetail.ProductId = RequestBasketDetailRequest.ProductId;
            newRequestBasketDetail.Quantity = RequestBasketDetailRequest.Quantity;


            await _RequestBasketDetailRepository.AddAsync(newRequestBasketDetail);
            return true;
        }

        public async Task<Boolean> UpdateRequestBasketDetail(UpdateRequestBasketDetailRequest RequestBasketDetailRequest)
        {
            var RequestBasketDetailUpdate = GetRequestBasketDetailByID(RequestBasketDetailRequest.RequestBasketId).Result;

            RequestBasketDetailUpdate.RequestBasketId = RequestBasketDetailRequest.RequestBasketId;
            RequestBasketDetailUpdate.ProductId = RequestBasketDetailRequest.ProductId;
            RequestBasketDetailUpdate.Quantity = RequestBasketDetailRequest.Quantity;

            await _RequestBasketDetailRepository.UpdateAsync(RequestBasketDetailUpdate);
            return true;
        }

        //public async Task<Boolean> DeleteRequestBasketDetail(RequestBasketDetail RequestBasketDetailDelete)
        //{
        //    RequestBasketDetailDelete.Status = 0;
        //    await _RequestBasketDetailRepository.UpdateAsync(RequestBasketDetailDelete);
        //    return true;
        //}

        public async Task<RequestBasketDetail> GetRequestBasketDetailByID(Guid? id)
        {
            return await _RequestBasketDetailRepository.GetFirstOrDefaultAsync(x => x.RequestBasketId == id);
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
