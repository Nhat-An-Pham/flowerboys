using Repository.Utils.Repository.Interface;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Models.Models;

namespace Traibanhoa.Modules.RequestBasketDetailModule.Interface
{
    public interface IRequestBasketDetailRepository : IRepository<RequestBasketDetail>
    {
        public Task<ICollection<RequestBasketDetail>> GetRequestBasketDetailsBy(
               Expression<Func<RequestBasketDetail, bool>> filter = null,
               Func<IQueryable<RequestBasketDetail>, ICollection<RequestBasketDetail>> options = null,
               string includeProperties = null
           );
    }
}
