using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Traibanhoa.Modules.OrderProductDetailModule.Interface;
using Traibanhoa.Modules.OrderProductDetailModule.Request;

namespace Traibanhoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderProductDetailsController : ControllerBase
    {
        private readonly IOrderProductDetailService _orderProductDetail;

        public OrderProductDetailsController(IOrderProductDetailService orderProductDetail)
        {
            _orderProductDetail = orderProductDetail;
        }

        // GET api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderProductDetail>>> GetOrderProductDetails()
        {
            try
            {
                var response = await _orderProductDetail.GetAll();
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderProductDetail>> GetOrderProductDetail([FromRoute] Guid id)
        {
            var orderProductDetail = await _orderProductDetail.GetOrderProductDetailByID(id);

            if (orderProductDetail == null)
            {
                return NotFound();
            }

            return orderProductDetail;
        }

        // POST api/<ValuesController>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PostOrderProductDetail([FromBody] CreateOrderProductDetailRequest createOrderProductDetailRequest)
        {
            var check = await _orderProductDetail.AddNewOrderProductDetail(createOrderProductDetailRequest);
            if (check) return Ok();
            else return BadRequest();
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderProductDetail>> PutOrderProductDetail([FromBody] UpdateOrderProductDetailRequest updateOrderProductDetailRequest)
        {
            var check = await _orderProductDetail.UpdateOrderProductDetail(updateOrderProductDetailRequest);
            if (check) return Ok();
            else return BadRequest();
        }

        //// DELETE: api/OrderProductDetails/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteOrderProductDetail(Guid id)
        //{
        //    var orderProductDetail = await _context.OrderProductDetails.FindAsync(id);
        //    if (orderProductDetail == null)
        //    {
        //        return NotFound();
        //    }

        //    _orderProductDetail.de
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

    }
}
