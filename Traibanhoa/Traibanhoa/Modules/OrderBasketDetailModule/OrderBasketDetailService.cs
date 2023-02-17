using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Traibanhoa.Modules.OrderBasketDetailModule.Interface;
using Models.Models;
using Traibanhoa.Modules.OrderBasketDetailModule.Request;

namespace Traibanhoa.Modules.OrderBasketDetailModule
{
    public class OrderBasketDetailService : IOrderBasketDetailService
    {
        private readonly IOrderBasketDetailRepository _OrderBasketDetailRepository;
        public OrderBasketDetailService(IOrderBasketDetailRepository OrderBasketDetailRepository)
        {
            _OrderBasketDetailRepository = OrderBasketDetailRepository;
        }

        public async Task<ICollection<OrderBasketDetail>> GetAll()
        {
            return await _OrderBasketDetailRepository.GetAll();
        }

        public Task<ICollection<OrderBasketDetail>> GetOrderBasketDetailsBy(
                Expression<Func<OrderBasketDetail,
                bool>> filter = null,
                Func<IQueryable<OrderBasketDetail>,
                ICollection<OrderBasketDetail>> options = null,
                string includeProperties = null)
        {
            return _OrderBasketDetailRepository.GetOrderBasketDetailsBy(filter);
        }


        public async Task<Boolean> AddNewOrderBasketDetail(CreateOrderBasketDetailRequest OrderBasketDetailRequest)
        {
            var newOrderBasketDetail = new OrderBasketDetail();

            if (_OrderBasketDetailRepository.GetFirstOrDefaultAsync(x => x.OrderId == OrderBasketDetailRequest.OrderId) == null)
            {
                return false;
            }
            newOrderBasketDetail.BasketId = OrderBasketDetailRequest.BasketId;
            newOrderBasketDetail.Quantity = OrderBasketDetailRequest.Quantity;
            newOrderBasketDetail.Price = OrderBasketDetailRequest.Price;
            newOrderBasketDetail.IsRequest = OrderBasketDetailRequest.IsRequest;

            await _OrderBasketDetailRepository.AddAsync(newOrderBasketDetail);
            return true;
        }

        public async Task<Boolean> UpdateOrderBasketDetail(UpdateOrderBasketDetailRequest OrderBasketDetailRequest)
        {
            var OrderBasketDetailUpdate = GetOrderBasketDetailByID(OrderBasketDetailRequest.OrderId).Result;

            OrderBasketDetailUpdate.BasketId = OrderBasketDetailRequest.BasketId;
            OrderBasketDetailUpdate.Quantity = OrderBasketDetailRequest.Quantity;
            OrderBasketDetailUpdate.Price = OrderBasketDetailRequest.Price;
            OrderBasketDetailUpdate.IsRequest = OrderBasketDetailRequest.IsRequest;

            await _OrderBasketDetailRepository.UpdateAsync(OrderBasketDetailUpdate);
            return true;
        }

        //public async Task<Boolean> DeleteOrderBasketDetail(OrderBasketDetail OrderBasketDetailDelete)
        //{
        //    OrderBasketDetailDelete.Status = 0;
        //    await _OrderBasketDetailRepository.UpdateAsync(OrderBasketDetailDelete);
        //    return true;
        //}

        public async Task<OrderBasketDetail> GetOrderBasketDetailByID(Guid? id)
        {
            return await _OrderBasketDetailRepository.GetFirstOrDefaultAsync(x => x.OrderId == id);
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
