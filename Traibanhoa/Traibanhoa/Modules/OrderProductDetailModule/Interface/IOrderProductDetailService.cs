using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Models.Models;
using Traibanhoa.Modules.OrderProductDetailModule.Request;

namespace Traibanhoa.Modules.OrderProductDetailModule.Interface
{
    public interface IOrderProductDetailService
    {
        public Task<ICollection<OrderProductDetail>> GetOrderProductDetailsBy(
            Expression<Func<OrderProductDetail, bool>> filter = null,
            Func<IQueryable<OrderProductDetail>, ICollection<OrderProductDetail>> options = null,
            string includeProperties = null);

        public Task<Guid?> AddNewOrderProductDetail(CreateOrderProductDetailRequest OrderProductDetailCreate);

        public Task UpdateOrderProductDetail(UpdateOrderProductDetailRequest OrderProductDetailUpdate);

        public Task DeleteOrderProductDetail(Guid? OorderProductDetailDeleteID);

        public Task<ICollection<OrderProductDetail>> GetAll();

        public Task<OrderProductDetail> GetOrderProductDetailByID(Guid? id);
    }
}
