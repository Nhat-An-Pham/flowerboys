using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Traibanhoa.Modules.OrderModule;
using Traibanhoa.Modules.OrderModule.Interface;
using Traibanhoa.Modules.OrderModule.Request;
using Traibanhoa.Modules.TypeModule.Request;

namespace Traibanhoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        #region Get all orders for staff include deleted, without paging
        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var res = await _orderService.GetAll();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get all orders by customer, without paging
        // GET: api/Orders/5
        [HttpGet("/Customer/{id}")]
        public async Task<ActionResult<Order>> GetByCustomer([FromRoute] Guid id)
        {
            try
            {
                var res = await _orderService.GetByCustomer(id);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        #endregion

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateOrderRequest request)
        {
            try
            {
                var order = new Order();
                order.OrderBy = request.OrderBy;
                order.Phonenumber = request.Phonenumber;
                order.PaymentMethod = request.PaymentMethod;
                order.TotalPrice = request.TotalPrice;
                order.Email = request.Email;
                order.ShippedAddress = request.ShippedAddress;
                order.IsRequest = request.IsRequest;

                var redirectUrl = await _orderService.AddNewOrder(order);
                return Ok(redirectUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("accept/{id}")]
        public async Task<ActionResult> AcceptOrder([FromRoute] Guid id)
        {
            try
            {
                await _orderService.AcceptOrder(id);
                return Ok("Order accepted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("deny/{id}")]
        public async Task<ActionResult> DenyAsync([FromRoute] Guid id)
        {
            try
            {
                await _orderService.DenyOrder(id);
                return Ok("Order denied");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("cancel/{id}")]
        public async Task<ActionResult> Cancel([FromRoute] Guid id)
        {
            try
            {
                await _orderService.CancelOrder(id);
                return Ok("Order cancelled");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("shipping/{id}")]
        public async Task<ActionResult> Shipping([FromRoute] Guid id)
        {
            try
            {
                await _orderService.Shipping(id);
                return Ok("Shipping");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("delivered/{id}")]
        public async Task<ActionResult> Delivered([FromRoute] Guid id, [FromQuery] bool fail)
        {
            try
            {
                if (fail)
                    await _orderService.DeliveredFail(id);
                else
                    await _orderService.Delivered(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("status")]
        public async Task<ActionResult> GetOrderResponse([FromQuery] int status)
        {
            try
            {
                var res = await _orderService.GetOrderResponse(status);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
