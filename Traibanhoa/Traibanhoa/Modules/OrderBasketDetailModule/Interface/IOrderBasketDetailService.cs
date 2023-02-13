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

        public Task<Boolean> AddNewOrderBasketDetail(CreateOrderBasketDetailRequest OrderBasketDetailCreate);

        public Task<Boolean> UpdateOrderBasketDetail(UpdateOrderBasketDetailRequest OrderBasketDetailUpdate);

        //public Task<Boolean> DeleteOrderBasketDetail(OrderBasketDetail OrderBasketDetailDelete);

        public Task<ICollection<OrderBasketDetail>> GetAll();

        public Task<OrderBasketDetail> GetOrderBasketDetailByID(Guid? id);
    }
}
