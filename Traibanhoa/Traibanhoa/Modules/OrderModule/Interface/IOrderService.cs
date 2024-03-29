﻿using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Models.Models;
using Traibanhoa.Modules.OrderModule.Request;
using Traibanhoa.Modules.OrderModule.Response;

namespace Traibanhoa.Modules.OrderModule.Interface
{
    public interface IOrderService
    {
        public Task<string> AddNewOrder(Order newOrder);

        public Task UpdateOrder(Order OrderUpdate);

        public Task<ICollection<Order>> GetAll();

        public Task<ICollection<OrderResponse>> GetOrderResponse(int status = -1);

        public Task<ICollection<Order>> GetByCustomer(Guid id);

        public Task<ICollection<Order>> GetOrdersBy(
            Expression<Func<Order, bool>> filter = null,
            Func<IQueryable<Order>, ICollection<Order>> options = null,
            string includeProperties = null);

        public Order GetOrderByID(Guid? id);

        public Task DeleteOrder(Guid id);

        public Task AcceptOrder(Guid id);

        public Task DenyOrder(Guid id);

        public Task CancelOrder(Guid id);

        public Task Shipping(Guid id);

        public Task Delivered(Guid id);

        public Task DeliveredFail(Guid id);

        public Task<string> PaymentWithPaypal(
            Guid orderId,
            string Cancel = null,
            string blogId = "",
            string PayerID = "",
            string guid = "");
    }
}
