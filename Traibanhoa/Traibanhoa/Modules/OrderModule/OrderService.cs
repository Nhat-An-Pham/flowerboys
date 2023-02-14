using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Traibanhoa.Modules.OrderModule.Interface;
using Models.Models;
using Traibanhoa.Modules.OrderModule.Request;

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
            return await _OrderRepository.GetAll();
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


        public async Task<Boolean> AddNewOrder(CreateOrderRequest OrderRequest)
        {
            var newOrder = new Order();

            if(_OrderRepository.GetFirstOrDefaultAsync(x => x.OrderId == OrderRequest.OrderId) == null)
            {
                return false;
            }
            newOrder.OrderDate= OrderRequest.OrderDate;
            newOrder.ShippedDate = OrderRequest.ShippedDate;
            newOrder.ShippedAddress = OrderRequest.ShippedAddress;
            newOrder.Phonenumber = OrderRequest.Phonenumber;
            newOrder.Email = OrderRequest.Email;
            newOrder.TotalPrice = OrderRequest.TotalPrice;
            newOrder.OrderStatus = OrderRequest.OrderStatus;
            newOrder.OrderBy = OrderRequest.OrderBy;
            newOrder.ConfirmBy = OrderRequest.ConfirmBy;

            await _OrderRepository.AddAsync(newOrder);
            return true;
        }

        public async Task<Boolean> UpdateOrder(UpdateOrderRequest OrderRequest)
        {
            var OrderUpdate = GetOrderByID(OrderRequest.OrderId).Result;

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
            return true;
        }

        //public async Task<Boolean> DeleteOrder(Order OrderDelete)
        //{
        //    OrderDelete.Status = 0;
        //    await _OrderRepository.UpdateAsync(OrderDelete);
        //    return true;
        //}

        public async Task<Order> GetOrderByID(Guid? id)
        {
            return await _OrderRepository.GetFirstOrDefaultAsync(x => x.OrderId == id);
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
