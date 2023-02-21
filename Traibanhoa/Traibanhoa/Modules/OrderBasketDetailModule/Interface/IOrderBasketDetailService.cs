using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Models.Models;
using Traibanhoa.Modules.OrderBasketDetailModule.Request;

namespace Traibanhoa.Modules.OrderBasketDetailModule.Interface
{
    public interface IOrderBasketDetailService
    {
        public Task<ICollection<OrderBasketDetail>> GetOrderBasketDetailsBy(
            Expression<Func<OrderBasketDetail, bool>> filter = null,
            Func<IQueryable<OrderBasketDetail>, ICollection<OrderBasketDetail>> options = null,
            string includeProperties = null);

        public Task<Guid?> AddNewOrderBasketDetail(CreateOrderBasketDetailRequest OrderBasketDetailCreate);

        public Task UpdateOrderBasketDetail(UpdateOrderBasketDetailRequest OrderBasketDetailUpdate);

        public Task DeleteOrderBasketDetail(Guid? orderBasketDetailDeleteID);

        public Task<ICollection<OrderBasketDetail>> GetAll();

        public Task<OrderBasketDetail> GetOrderBasketDetailByID(Guid? id);
    }
}
