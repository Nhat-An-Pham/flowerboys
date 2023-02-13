using Repository.Utils.Repository.Interface;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Models.Models;

namespace Traibanhoa.Modules.RequestBasketModule.Interface
{
    public interface IRequestBasketRepository : IRepository<RequestBasket>
    {
        public Task<ICollection<RequestBasket>> GetRequestBasketsBy(
               Expression<Func<RequestBasket, bool>> filter = null,
               Func<IQueryable<RequestBasket>, ICollection<RequestBasket>> options = null,
               string includeProperties = null
           );
    }
}
