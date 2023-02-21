using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Traibanhoa.Modules.OrderBasketDetailModule.Interface;
using Models.Models;
using Traibanhoa.Modules.OrderBasketDetailModule.Request;
using Models.Constant;
using Traibanhoa.Modules.TypeModule.Request;
using FluentValidation.Results;

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
            return await _OrderBasketDetailRepository.GetAll(options: o => o.OrderByDescending(x => x.Quantity != 0).ToList());
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


        public async Task<Guid?> AddNewOrderBasketDetail(CreateOrderBasketDetailRequest OrderBasketDetailRequest)
        {
            ValidationResult result = new CreateOrderBasketDetailRequestValidator().Validate(OrderBasketDetailRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            var newOrderBasketDetail = new OrderBasketDetail();

            newOrderBasketDetail.OrderId = OrderBasketDetailRequest.OrderId;
            newOrderBasketDetail.BasketId = OrderBasketDetailRequest.BasketId;
            newOrderBasketDetail.Quantity = OrderBasketDetailRequest.Quantity;
            newOrderBasketDetail.Price = OrderBasketDetailRequest.Price;
            newOrderBasketDetail.IsRequest = OrderBasketDetailRequest.IsRequest;

            await _OrderBasketDetailRepository.AddAsync(newOrderBasketDetail);
            return newOrderBasketDetail.OrderId;
        }

        public async Task UpdateOrderBasketDetail(UpdateOrderBasketDetailRequest OrderBasketDetailRequest)
        {
            try
            {
                var OrderBasketDetailUpdate = GetOrderBasketDetailByID(OrderBasketDetailRequest.OrderId).Result;

                if (OrderBasketDetailUpdate == null)
                {
                    throw new Exception(ErrorMessage.OrderError.ORDER_NOT_FOUND);
                }

                ValidationResult result = new UpdateOrderBasketDetailRequestValidator().Validate(OrderBasketDetailRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                OrderBasketDetailUpdate.BasketId = OrderBasketDetailRequest.BasketId;
                OrderBasketDetailUpdate.Quantity = OrderBasketDetailRequest.Quantity;
                OrderBasketDetailUpdate.Price = OrderBasketDetailRequest.Price;
                OrderBasketDetailUpdate.IsRequest = OrderBasketDetailRequest.IsRequest;

                await _OrderBasketDetailRepository.UpdateAsync(OrderBasketDetailUpdate);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }
            
        }

        public async Task DeleteOrderBasketDetail(Guid? orderBasketDetailDeleteID)
        {
            try
            {
                if (orderBasketDetailDeleteID == null)
                {
                    throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
                }

                OrderBasketDetail orderBasketDetailDelete = _OrderBasketDetailRepository.GetFirstOrDefaultAsync(x => x.BasketId == orderBasketDetailDeleteID && x.Quantity != 0).Result;

                if (orderBasketDetailDelete == null)
                {
                    throw new Exception(ErrorMessage.BasketError.BASKET_NOT_FOUND);
                }

                orderBasketDetailDelete.Quantity = 0;
                await _OrderBasketDetailRepository.UpdateAsync(orderBasketDetailDelete);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at delete type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<OrderBasketDetail> GetOrderBasketDetailByID(Guid? id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var orderBasketDetail = await _OrderBasketDetailRepository.GetFirstOrDefaultAsync(x => x.OrderId == id);
            if (orderBasketDetail == null)
            {
                throw new Exception(ErrorMessage.OrderError.ORDER_EXISTED);
            }
            return orderBasketDetail;
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
