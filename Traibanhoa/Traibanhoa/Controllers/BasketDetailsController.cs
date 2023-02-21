using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Traibanhoa.Modules.BasketDetailModule.Interface;
using Traibanhoa.Modules.BasketDetailModule.Request;
using Traibanhoa.Modules.TypeModule.Request;

namespace Traibanhoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketDetailsController : ControllerBase
    {
        private readonly IBasketDetailService _basketDetailService;

        public BasketDetailsController(IBasketDetailService basketDetailService)
        {
            _basketDetailService = basketDetailService;
        }

        // GET api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BasketDetail>>> GetBasketDetails()
        {
            try
            {
                var response = await _basketDetailService.GetAll();
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BasketDetail>> GetBasketDetail([FromRoute] Guid id)
        {
            try
            {
                return Ok(await _basketDetailService.GetBasketDetailByID(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<ValuesController>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BasketDetail>> PostBasketDetail([FromBody] CreateBasketDetailRequest createBasketDetailRequest)
        {
            try
            {
                return Ok(await _basketDetailService.AddNewBasketDetail(createBasketDetailRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutBasketDetail([FromBody] UpdateBasketDetailRequest updateBasketDetailRequest)
        {
            try
            {
                await _basketDetailService.UpdateBasketDetail(updateBasketDetailRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/BasketDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasketDetail([FromRoute] Guid id)
        {
            try
            {
                await _basketDetailService.DeleteBasketDetail(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
