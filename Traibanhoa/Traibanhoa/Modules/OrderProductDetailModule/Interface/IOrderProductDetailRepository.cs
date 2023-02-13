using Repository.Utils.Repository.Interface;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Models.Models;

namespace Traibanhoa.Modules.OrderProductDetailModule.Interface
{
    public interface IOrderProductDetailRepository : IRepository<OrderProductDetail>
    {
        public Task<ICollection<OrderProductDetail>> GetOrderProductDetailsBy(
               Expression<Func<OrderProductDetail, bool>> filter = null,
               Func<IQueryable<OrderProductDetail>, ICollection<OrderProductDetail>> options = null,
               string includeProperties = null
           );
    }
}
