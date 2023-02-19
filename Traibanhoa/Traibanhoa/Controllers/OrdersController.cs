using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Traibanhoa.Modules.OrderModule.Interface;
using Traibanhoa.Modules.OrderModule.Request;

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
            var order = await _orderSevice.GetOrderByID(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // POST api/<ValuesController>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PostOrder([FromBody] CreateOrderRequest createOrderRequest)
        {
            var check = await _orderSevice.AddNewOrder(createOrderRequest);
            if (check) return Ok();
            else return BadRequest();
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PutOrder([FromBody] UpdateOrderRequest updateOrderRequest)
        {
            var check = await _orderSevice.UpdateOrder(updateOrderRequest);
            if (check) return Ok();
            else return BadRequest();
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
