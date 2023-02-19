using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Traibanhoa.Modules.OrderBasketDetailModule.Interface;
using Traibanhoa.Modules.OrderBasketDetailModule.Request;

namespace Traibanhoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderBasketDetailsController : ControllerBase
    {
        private readonly IOrderBasketDetailService _orderbasketDetailService;

        public OrderBasketDetailsController(IOrderBasketDetailService orderbasketDetailService)
        {
            _orderbasketDetailService = orderbasketDetailService;
        }

        // GET api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderBasketDetail>>> GetOrderBasketDetails()
        {
            try
            {
                var response = await _orderbasketDetailService.GetAll();
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderBasketDetail>> GetOrderBasketDetail([FromRoute]Guid id)
        {
            var orderBasketDetail = await _orderbasketDetailService.GetOrderBasketDetailByID(id);

            if (orderBasketDetail == null)
            {
                return NotFound();
            }

            return orderBasketDetail;
        }

        // POST api/<ValuesController>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PostOrderBasketDetail([FromBody] CreateOrderBasketDetailRequest createOrderBasketDetailRequest)
        {
            var check = await _orderbasketDetailService.AddNewOrderBasketDetail(createOrderBasketDetailRequest);
            if (check) return Ok();
            else return BadRequest();
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderBasketDetail>> PutOrderBasketDetail([FromBody] UpdateOrderBasketDetailRequest updateOrderBasketDetailRequest)
        {
            var check = await _orderbasketDetailService.UpdateOrderBasketDetail(updateOrderBasketDetailRequest);
            if (check) return Ok();
            else return BadRequest();
        }

        //// DELETE: api/OrderBasketDetails/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteOrderBasketDetail([FromRoute] Guid id)
        //{
        //    var orderBasketDetail = await _orderbasketDetailService.de(id);
        //    if (orderBasketDetail == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.OrderBasketDetails.Remove(orderBasketDetail);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        
    }
}
