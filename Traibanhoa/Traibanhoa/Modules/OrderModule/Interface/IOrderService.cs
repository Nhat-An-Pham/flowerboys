using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Models.Models;
using Traibanhoa.Modules.OrderModule.Request;

namespace Traibanhoa.Modules.OrderModule.Interface
{
    public interface IOrderService
    {
        public Task<ICollection<Order>> GetOrdersBy(
            Expression<Func<Order, bool>> filter = null,
            Func<IQueryable<Order>, ICollection<Order>> options = null,
            string includeProperties = null);

        public Task<Boolean> AddNewOrder(CreateOrderRequest OrderCreate);

        public Task<Boolean> UpdateOrder(UpdateOrderRequest OrderUpdate);

        //public Task<Boolean> DeleteOrder(Order OrderDelete);

        public Task<ICollection<Order>> GetAll();

        public Task<Order> GetOrderByID(Guid? id);
    }
}
