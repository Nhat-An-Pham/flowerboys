using Repository.Utils.Repository.Interface;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Models.Models;

namespace Traibanhoa.Modules.BasketDetailModule.Interface
{
    public interface IBasketDetailRepository : IRepository<BasketDetail>
    {
        public Task<ICollection<BasketDetail>> GetBasketDetailsBy(
               Expression<Func<BasketDetail, bool>> filter = null,
               Func<IQueryable<BasketDetail>, ICollection<BasketDetail>> options = null,
               string includeProperties = null
           );
    }
}
