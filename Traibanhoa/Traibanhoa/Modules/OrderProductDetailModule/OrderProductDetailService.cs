using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Traibanhoa.Modules.OrderProductDetailModule.Interface;
using Models.Models;
using Traibanhoa.Modules.OrderProductDetailModule.Request;

namespace Traibanhoa.Modules.OrderProductDetailModule
{
    public class OrderProductDetailDetailService : IOrderProductDetailService
    {
        private readonly IOrderProductDetailRepository _OrderProductDetailRepository;
        public OrderProductDetailDetailService(IOrderProductDetailRepository OrderProductDetailRepository)
        {
            _OrderProductDetailRepository = OrderProductDetailRepository;
        }

        public async Task<ICollection<OrderProductDetail>> GetAll()
        {
            return await _OrderProductDetailRepository.GetAll();
        }

        public Task<ICollection<OrderProductDetail>> GetOrderProductDetailsBy(
                Expression<Func<OrderProductDetail,
                bool>> filter = null,
                Func<IQueryable<OrderProductDetail>,
                ICollection<OrderProductDetail>> options = null,
                string includeProperties = null)
        {
            return _OrderProductDetailRepository.GetOrderProductDetailsBy(filter);
        }


        public async Task<Boolean> AddNewOrderProductDetail(CreateOrderProductDetailRequest OrderProductDetailRequest)
        {
            var newOrderProductDetail = new OrderProductDetail();

            if (_OrderProductDetailRepository.GetFirstOrDefaultAsync(x => x.OrderId == OrderProductDetailRequest.OrderId) == null)
            {
                return false;
            }
            newOrderProductDetail.ProductId = OrderProductDetailRequest.ProductId;
            newOrderProductDetail.Quantity = OrderProductDetailRequest.Quantity;
            newOrderProductDetail.Price = OrderProductDetailRequest.Price;

            await _OrderProductDetailRepository.AddAsync(newOrderProductDetail);
            return true;
        }

        public async Task<Boolean> UpdateOrderProductDetail(UpdateOrderProductDetailRequest OrderProductDetailRequest)
        {
            var OrderProductDetailUpdate = GetOrderProductDetailByID(OrderProductDetailRequest.OrderId).Result;

            OrderProductDetailUpdate.ProductId = OrderProductDetailRequest.ProductId;
            OrderProductDetailUpdate.Quantity = OrderProductDetailRequest.Quantity;
            OrderProductDetailUpdate.Price = OrderProductDetailRequest.Price;

            await _OrderProductDetailRepository.UpdateAsync(OrderProductDetailUpdate);
            return true;
        }

        //public async Task<Boolean> DeleteOrderProductDetail(OrderProductDetail OrderProductDetailDelete)
        //{
        //    OrderProductDetailDelete.Status = 0;
        //    await _OrderProductDetailRepository.UpdateAsync(OrderProductDetailDelete);
        //    return true;
        //}

        public async Task<OrderProductDetail> GetOrderProductDetailByID(Guid? id)
        {
            return await _OrderProductDetailRepository.GetFirstOrDefaultAsync(x => x.OrderId == id);
        }

        //public async Task<ICollection<TypeDropdownResponse>> GetTypeDropdown()
        //{
        //    var result = await _typeRepository.GetTypesBy(x => x.Status == true);
        //    return result.Select(x => new TypeDropdownResponse
        //    {
        //        TypeId = x.TypeId,
        //        TypeName = x.Name
        //    }).ToList();
        //}
    }
}
