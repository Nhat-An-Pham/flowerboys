using Repository.Utils.Repository.Interface;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Models.Models;

namespace Traibanhoa.Modules.OrderBasketDetailModule.Interface
{
    public interface IOrderBasketDetailRepository : IRepository<OrderBasketDetail>
    {
        public Task<ICollection<OrderBasketDetail>> GetOrderBasketDetailsBy(
               Expression<Func<OrderBasketDetail, bool>> filter = null,
               Func<IQueryable<OrderBasketDetail>, ICollection<OrderBasketDetail>> options = null,
               string includeProperties = null
           );
    }
}
