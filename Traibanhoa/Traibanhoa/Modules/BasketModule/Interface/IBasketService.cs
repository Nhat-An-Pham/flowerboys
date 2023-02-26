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

        public Task<Guid?> AddNewBasket(CreateBasketRequest BasketCreate);

        public Task UpdateBasket(UpdateBasketRequest BasketUpdate);

        public Task DeleteBasket(Guid? basketDeleteID);

        public Task<ICollection<Basket>> GetAll();

        public Task<Basket> GetBasketByID(Guid? id);
    }
}
