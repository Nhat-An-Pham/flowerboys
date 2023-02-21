using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Traibanhoa.Modules.OrderBasketDetailModule.Interface;
using Traibanhoa.Modules.OrderBasketDetailModule.Request;
using Traibanhoa.Modules.TypeModule.Request;

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
            try
            {
                return Ok(await _orderbasketDetailService.GetOrderBasketDetailByID(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<ValuesController>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderBasketDetail>> PostOrderBasketDetail([FromBody] CreateOrderBasketDetailRequest createOrderBasketDetailRequest)
        {
            try
            {
                return Ok(await _orderbasketDetailService.AddNewOrderBasketDetail(createOrderBasketDetailRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutOrderBasketDetail([FromBody] UpdateOrderBasketDetailRequest updateOrderBasketDetailRequest)
        {
            try
            {
                await _orderbasketDetailService.UpdateOrderBasketDetail(updateOrderBasketDetailRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        // DELETE: api/OrderBasketDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderBasketDetail([FromRoute] Guid id)
        {
            try
            {
                await _orderbasketDetailService.DeleteOrderBasketDetail(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
