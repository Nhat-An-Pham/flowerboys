using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Traibanhoa.Modules.BasketModule.Interface;
using Models.Models;
using Traibanhoa.Modules.BasketModule.Request;

namespace Traibanhoa.Modules.BasketModule
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _BasketRepository;
        public BasketService(IBasketRepository BasketRepository)
        {
            _BasketRepository = BasketRepository;
        }

        public async Task<ICollection<Basket>> GetAll()
        {
            return await _BasketRepository.GetAll();
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


        public async Task<Boolean> AddNewBasket(CreateBasketRequest BasketRequest)
        {
            var newBasket = new Basket();

            if(_BasketRepository.GetFirstOrDefaultAsync(x => x.BasketId == BasketRequest.BasketId) == null)
            {
                return false;
            }
            newBasket.BasketId= Guid.NewGuid();
            newBasket.Title = BasketRequest.Title;
            newBasket.Description = BasketRequest.Description;
            newBasket.ImageUrl = BasketRequest.ImageUrl;
            newBasket.View = BasketRequest.View;
            newBasket.BasketPrice = BasketRequest.BasketPrice;
            newBasket.Status = BasketRequest.Status;
            newBasket.CreatedDate = DateTime.Now;
            newBasket.UpdatedDate = null;

            await _BasketRepository.AddAsync(newBasket);
            return true;
        }

        public async Task<Boolean> UpdateBasket(UpdateBasketRequest BasketRequest)
        {
            var BasketUpdate = GetBasketByID(BasketRequest.BasketId).Result;

            BasketUpdate.Title = BasketRequest.Title;
            BasketUpdate.Description = BasketRequest.Description;
            BasketUpdate.ImageUrl = BasketRequest.ImageUrl;
            BasketUpdate.View = BasketRequest.View;
            BasketUpdate.BasketPrice = BasketRequest.BasketPrice;
            BasketUpdate.Status = BasketRequest.Status;
            BasketUpdate.UpdatedDate = DateTime.Now;

            await _BasketRepository.UpdateAsync(BasketUpdate);
            return true;
        }

        public async Task<Boolean> DeleteBasket(Basket BasketDelete)
        {
            BasketDelete.Status = 0;
            await _BasketRepository.UpdateAsync(BasketDelete);
            return true;
        }

        public async Task<Basket> GetBasketByID(Guid? id)
        {
            return await _BasketRepository.GetFirstOrDefaultAsync(x => x.BasketId == id);
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
