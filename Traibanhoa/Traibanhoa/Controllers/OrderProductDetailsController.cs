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
            try
            {
                return Ok(await _orderProductDetail.GetOrderProductDetailByID(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<ValuesController>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderProductDetail>> PostOrderProductDetail([FromBody] CreateOrderProductDetailRequest createOrderProductDetailRequest)
        {
            try
            {
                return Ok(await _orderProductDetail.AddNewOrderProductDetail(createOrderProductDetailRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutOrderProductDetail([FromBody] UpdateOrderProductDetailRequest updateOrderProductDetailRequest)
        {
            try
            {
                await _orderProductDetail.UpdateOrderProductDetail(updateOrderProductDetailRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/OrderProductDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderProductDetail(Guid id)
        {
            try
            {
                await _orderProductDetail.DeleteOrderProductDetail(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
