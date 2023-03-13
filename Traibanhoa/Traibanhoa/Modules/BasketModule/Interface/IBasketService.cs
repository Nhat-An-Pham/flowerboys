using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Models.Models;
using Traibanhoa.Modules.BasketModule.Request;
using Traibanhoa.Modules.BasketModule.Response;

namespace Traibanhoa.Modules.BasketModule.Interface
{
    public interface IBasketService
    {
        public Task<ICollection<Basket>> GetBasketsBy(
            Expression<Func<Basket, bool>> filter = null,
            Func<IQueryable<Basket>, ICollection<Basket>> options = null,
            string includeProperties = null);

        public Task<Guid> AddNewBasket();

        public Task UpdateBasket(UpdateBasketRequest BasketUpdate);

        public Task DeleteBasket(Guid? basketDeleteID);
        public Task RestoreBasket(Guid? basketRestoreID);

        public Task<ICollection<GetBasketResponse>> GetAll();
        public Task<ICollection<HomeNewBasketResponse>> GetNewBasketsForHome();
        public Task<ICollection<DetailHomeViewBasketResponse>> GetMostViewBaskets();
        public Task<ICollection<DetailHomeViewBasketResponse>> GetBasketsByPrice();
        public Task<ICollection<SearchBasketResponse>> GetBasketByName(String name);
        public Task<Basket> GetBasketByID(Guid? id);
    }
}
