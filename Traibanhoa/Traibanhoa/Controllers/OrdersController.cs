using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Traibanhoa.Modules.OrderModule.Interface;
using Traibanhoa.Modules.OrderModule.Request;
using Traibanhoa.Modules.TypeModule.Request;

namespace Traibanhoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderSevice;

        public OrdersController(IOrderService orderSevice)
        {
            _orderSevice = orderSevice;
        }

        // GET api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            try
            {
                var response = await _orderSevice.GetAll();
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder([FromRoute]Guid id)
        {
            try
            {
                return Ok(await _orderSevice.GetOrderByID(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<ValuesController>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder([FromBody] CreateOrderRequest createOrderRequest)
        {
            try
            {
                return Ok(await _orderSevice.AddNewOrder(createOrderRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutOrder([FromBody] UpdateOrderRequest updateOrderRequest)
        {
            try
            {
                await _orderSevice.UpdateOrder(updateOrderRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //// DELETE: api/Orders/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteOrder(Guid id)
        //{
        //    var order = await _context.Orders.FindAsync(id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    _orderSevice.de
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool OrderExists(Guid id)
        //{
        //    return _context.Orders.Any(e => e.OrderId == id);
        //}
    }
}
