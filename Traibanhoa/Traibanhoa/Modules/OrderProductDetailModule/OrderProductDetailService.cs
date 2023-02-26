using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Traibanhoa.Modules.OrderProductDetailModule.Interface;
using Models.Models;
using Traibanhoa.Modules.OrderProductDetailModule.Request;
using Models.Constant;
using Traibanhoa.Modules.TypeModule.Request;
using FluentValidation.Results;
using FluentValidation;

namespace Traibanhoa.Modules.OrderProductDetailModule
{
    public class OrderProductDetailService : IOrderProductDetailService
    {
        private readonly IOrderProductDetailRepository _OrderProductDetailRepository;
        public OrderProductDetailService(IOrderProductDetailRepository OrderProductDetailRepository)
        {
            _OrderProductDetailRepository = OrderProductDetailRepository;
        }

        public async Task<ICollection<OrderProductDetail>> GetAll()
        {
            return await _OrderProductDetailRepository.GetAll(options: o => o.OrderByDescending(x => x.Quantity != 0).ToList());
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


        public async Task<Guid?> AddNewOrderProductDetail(CreateOrderProductDetailRequest OrderProductDetailRequest)
        {
            ValidationResult result = new CreateOrderProductDetailRequestValidator().Validate(OrderProductDetailRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            var newOrderProductDetail = new OrderProductDetail();

            newOrderProductDetail.OrderId = OrderProductDetailRequest.OrderId;
            newOrderProductDetail.ProductId = OrderProductDetailRequest.ProductId;
            newOrderProductDetail.Quantity = OrderProductDetailRequest.Quantity;
            newOrderProductDetail.Price = OrderProductDetailRequest.Price;

            await _OrderProductDetailRepository.AddAsync(newOrderProductDetail);
            return newOrderProductDetail.OrderId;
        }

        public async Task UpdateOrderProductDetail(UpdateOrderProductDetailRequest OrderProductDetailRequest)
        {
            try
            {
                var OrderProductDetailUpdate = GetOrderProductDetailByID(OrderProductDetailRequest.OrderId).Result;

                if (OrderProductDetailUpdate == null)
                {
                    throw new Exception(ErrorMessage.OrderError.ORDER_NOT_FOUND);
                }

                ValidationResult result = new UpdateOrderProductDetailRequestValidator().Validate(OrderProductDetailRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                OrderProductDetailUpdate.OrderId = OrderProductDetailRequest.OrderId;
                OrderProductDetailUpdate.ProductId = OrderProductDetailRequest.ProductId;
                OrderProductDetailUpdate.Quantity = OrderProductDetailRequest.Quantity;
                OrderProductDetailUpdate.Price = OrderProductDetailRequest.Price;

                await _OrderProductDetailRepository.UpdateAsync(OrderProductDetailUpdate);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteOrderProductDetail(Guid? orderProductDetailDeleteID)
        {
            try
            {
                if (orderProductDetailDeleteID == null)
                {
                    throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
                }

                OrderProductDetail orderProductDetailDelete = _OrderProductDetailRepository.GetFirstOrDefaultAsync(x => x.ProductId == orderProductDetailDeleteID && x.Quantity != 0).Result;

                if (orderProductDetailDelete == null)
                {
                    throw new Exception(ErrorMessage.ProductError.PRODUCT_NOT_FOUND);
                }

                orderProductDetailDelete.Quantity = 0;
                await _OrderProductDetailRepository.UpdateAsync(orderProductDetailDelete);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at delete type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<OrderProductDetail> GetOrderProductDetailByID(Guid? id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var orderProductDetail = await _OrderProductDetailRepository.GetFirstOrDefaultAsync(x => x.OrderId == id);
            if (orderProductDetail == null)
            {
                throw new Exception(ErrorMessage.OrderError.ORDER_NOT_FOUND);
            }
            return orderProductDetail;
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
