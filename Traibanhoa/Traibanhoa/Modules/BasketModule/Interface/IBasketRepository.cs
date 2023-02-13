using Repository.Utils.Repository.Interface;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Models.Models;

namespace Traibanhoa.Modules.BasketModule.Interface
{
    public interface IBasketRepository : IRepository<Basket>
    {
        public Task<ICollection<Basket>> GetBasketsBy(
               Expression<Func<Basket, bool>> filter = null,
               Func<IQueryable<Basket>, ICollection<Basket>> options = null,
               string includeProperties = null
           );
    }
}
