using Repository.Utils.Repository.Interface;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Models.Models;

namespace Traibanhoa.Modules.OrderModule.Interface
{
    public interface IOrderRepository : IRepository<Order>
    {
        public Task<ICollection<Order>> GetOrdersBy(
               Expression<Func<Order, bool>> filter = null,
               Func<IQueryable<Order>, ICollection<Order>> options = null,
               string includeProperties = null
           );
    }
}
