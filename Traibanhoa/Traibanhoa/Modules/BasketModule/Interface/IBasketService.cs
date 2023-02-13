using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Models.Models;
using Traibanhoa.Modules.BasketModule.Request;

namespace Traibanhoa.Modules.BasketModule.Interface
{
    public interface IBasketService
    {
        public Task<ICollection<Basket>> GetBasketsBy(
            Expression<Func<Basket, bool>> filter = null,
            Func<IQueryable<Basket>, ICollection<Basket>> options = null,
            string includeProperties = null);

        public Task<Boolean> AddNewBasket(CreateBasketRequest BasketCreate);

        public Task<Boolean> UpdateBasket(UpdateBasketRequest BasketUpdate);

        public Task<Boolean> DeleteBasket(Basket BasketDelete);

        public Task<ICollection<Basket>> GetAll();

        public Task<Basket> GetBasketByID(Guid? id);
    }
}
