using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Traibanhoa.Modules.OrderModule.Interface;
using Models.Models;
using Traibanhoa.Modules.OrderModule.Request;
using Models.Constant;
using Traibanhoa.Modules.TypeModule.Request;
using FluentValidation.Results;

namespace Traibanhoa.Modules.OrderModule
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _OrderRepository;
        public OrderService(IOrderRepository OrderRepository)
        {
            _OrderRepository = OrderRepository;
        }

        public async Task<ICollection<Order>> GetAll()
        {
            return await _OrderRepository.GetAll(options: o => o.OrderByDescending(x => x.OrderDate).ToList());
        }

        public Task<ICollection<Order>> GetOrdersBy(
                Expression<Func<Order,
                bool>> filter = null,
                Func<IQueryable<Order>,
                ICollection<Order>> options = null,
                string includeProperties = null)
        {
            return _OrderRepository.GetOrdersBy(filter);
        }


        public async Task<Guid?> AddNewOrder(CreateOrderRequest OrderRequest)
        {
            ValidationResult result = new CreateOrderRequestValidator().Validate(OrderRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            var newOrder = new Order();

            newOrder.OrderId = Guid.NewGuid();
            newOrder.OrderDate = DateTime.Now;
            newOrder.ShippedDate = OrderRequest.ShippedDate;
            newOrder.ShippedAddress = OrderRequest.ShippedAddress;
            newOrder.Phonenumber = OrderRequest.Phonenumber;
            newOrder.Email = OrderRequest.Email;
            newOrder.TotalPrice = OrderRequest.TotalPrice;
            newOrder.OrderStatus = OrderRequest.OrderStatus;
            newOrder.OrderBy = OrderRequest.OrderBy;
            newOrder.ConfirmBy = OrderRequest.ConfirmBy;

            await _OrderRepository.AddAsync(newOrder);
            return newOrder.OrderId;
        }

        public async Task UpdateOrder(UpdateOrderRequest OrderRequest)
        {
            try
            {
                var OrderUpdate = GetOrderByID(OrderRequest.OrderId).Result;

                if (OrderUpdate == null)
                {
                    throw new Exception(ErrorMessage.OrderError.ORDER_NOT_FOUND);
                }

                ValidationResult result = new UpdateOrderRequestValidator().Validate(OrderRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                OrderUpdate.OrderDate = OrderRequest.OrderDate;
                OrderUpdate.ShippedDate = OrderRequest.ShippedDate;
                OrderUpdate.ShippedAddress = OrderRequest.ShippedAddress;
                OrderUpdate.Phonenumber = OrderRequest.Phonenumber;
                OrderUpdate.Email = OrderRequest.Email;
                OrderUpdate.TotalPrice = OrderRequest.TotalPrice;
                OrderUpdate.OrderStatus = OrderRequest.OrderStatus;
                OrderUpdate.OrderBy = OrderRequest.OrderBy;
                OrderUpdate.ConfirmBy = OrderRequest.ConfirmBy;

                await _OrderRepository.UpdateAsync(OrderUpdate);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        //public async Task<Boolean> DeleteOrder(Order OrderDelete)
        //{
        //    OrderDelete.Status = 0;
        //    await _OrderRepository.UpdateAsync(OrderDelete);
        //    return true;
        //}

        public async Task<Order> GetOrderByID(Guid? id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var order = await _OrderRepository.GetFirstOrDefaultAsync(x => x.OrderId == id);
            if (order == null)
            {
                throw new Exception(ErrorMessage.OrderError.ORDER_NOT_FOUND);
            }
            return order;
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
