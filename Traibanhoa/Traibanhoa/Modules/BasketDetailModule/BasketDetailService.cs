using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Models.Models;
using Traibanhoa.Modules.BasketDetailModule.Request;
using Traibanhoa.Modules.BasketDetailModule.Interface;

namespace Traibanhoa.Modules.BasketDetailModule
{
    public class BasketDetailDetailService : IBasketDetailService
    {
        private readonly IBasketDetailRepository _BasketDetailRepository;
        public BasketDetailDetailService(IBasketDetailRepository BasketDetailRepository)
        {
            _BasketDetailRepository = BasketDetailRepository;
        }

        public async Task<ICollection<BasketDetail>> GetAll()
        {
            return await _BasketDetailRepository.GetAll();
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


        public async Task<Boolean> AddNewBasketDetail(CreateBasketDetailRequest BasketDetailRequest)
        {
            var newBasketDetail = new BasketDetail();

            //if(_BasketDetailRepository.GetFirstOrDefaultAsync(x => x.BasketDetailId == BasketDetailRequest.BasketDetailId) == null)
            //{
            //    return false;
            //}
            newBasketDetail.BasketId= BasketDetailRequest.BasketId;
            newBasketDetail.ProductId = BasketDetailRequest.ProductId;
            newBasketDetail.Quantity = BasketDetailRequest.Quantity;

            await _BasketDetailRepository.AddAsync(newBasketDetail);
            return true;
        }

        public async Task<Boolean> UpdateBasketDetail(UpdateBasketDetailRequest BasketDetailRequest)
        {
            var BasketDetailUpdate = GetBasketDetailByID(BasketDetailRequest.BasketId).Result;

            BasketDetailUpdate.BasketId = BasketDetailRequest.BasketId;
            BasketDetailUpdate.ProductId = BasketDetailRequest.ProductId;
            BasketDetailUpdate.Quantity = BasketDetailRequest.Quantity;

            await _BasketDetailRepository.UpdateAsync(BasketDetailUpdate);
            return true;
        }

        //public async Task<Boolean> DeleteBasketDetail(BasketDetail BasketDetailDelete)
        //{
        //    BasketDetailDelete.Status = 0;
        //    await _BasketDetailRepository.UpdateAsync(BasketDetailDelete);
        //    return true;
        //}

        public async Task<BasketDetail> GetBasketDetailByID(Guid? id)
        {
            return await _BasketDetailRepository.GetFirstOrDefaultAsync(x => x.BasketId == id);
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
