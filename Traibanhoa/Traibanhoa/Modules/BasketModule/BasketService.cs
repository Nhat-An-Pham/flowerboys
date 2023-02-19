using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Traibanhoa.Modules.BasketModule.Interface;
using Models.Models;
using Traibanhoa.Modules.BasketModule.Request;
using Models.Constant;
using Traibanhoa.Modules.TypeModule.Request;
using FluentValidation;
using FluentValidation.Results;

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
            return await _BasketRepository.GetAll(options: o => o.OrderByDescending(x => x.UpdatedDate).ToList());
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


        public async Task<Guid?> AddNewBasket(CreateBasketRequest BasketRequest)
        {
            ValidationResult result = new CreateBasketRequestValidator().Validate(BasketRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            var newBasket = new Basket();

            newBasket.BasketId= Guid.NewGuid();
            newBasket.Title = BasketRequest.Title;
            newBasket.Description = BasketRequest.Description;
            newBasket.ImageUrl = BasketRequest.ImageUrl;
            newBasket.View = BasketRequest.View;
            newBasket.BasketPrice = BasketRequest.BasketPrice;
            newBasket.Status = BasketRequest.Status;
            newBasket.CreatedDate = DateTime.Now;
            newBasket.UpdatedDate = DateTime.Now;

            await _BasketRepository.AddAsync(newBasket);
            return newBasket.BasketId;
        }

        public async Task UpdateBasket(UpdateBasketRequest BasketRequest)
        {
            try
            {
                var BasketUpdate = GetBasketByID(BasketRequest.BasketId).Result;

                if (BasketUpdate == null)
                {
                    throw new Exception(ErrorMessage.BasketError.BASKET_NOT_FOUND);
                }

                ValidationResult result = new UpdateBasketRequestValidator().Validate(BasketRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                BasketUpdate.Title = BasketRequest.Title;
                BasketUpdate.Description = BasketRequest.Description;
                BasketUpdate.ImageUrl = BasketRequest.ImageUrl;
                BasketUpdate.View = BasketRequest.View;
                BasketUpdate.BasketPrice = BasketRequest.BasketPrice;
                BasketUpdate.Status = BasketRequest.Status;
                BasketUpdate.UpdatedDate = DateTime.Now;

                await _BasketRepository.UpdateAsync(BasketUpdate);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
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

                Basket basketDelete = _BasketRepository.GetFirstOrDefaultAsync(x => x.BasketId == basketDeleteID && x.Status == 0).Result;

                if (basketDelete == null)
                {
                    throw new Exception(ErrorMessage.BasketError.BASKET_NOT_FOUND);
                }

                basketDelete.Status = 0;
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
