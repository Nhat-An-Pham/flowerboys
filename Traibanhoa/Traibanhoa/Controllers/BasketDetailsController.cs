using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Traibanhoa.Modules.BasketDetailModule.Interface;
using Traibanhoa.Modules.BasketDetailModule.Request;

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
            var basketDetail = await _basketDetailService.GetBasketDetailByID(id);

            if (basketDetail == null)
            {
                return NotFound();
            }

            return basketDetail;
        }

        // POST api/<ValuesController>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PostBasketDetail([FromBody] CreateBasketDetailRequest createBasketDetailRequest)
        {
            var check = await _basketDetailService.AddNewBasketDetail(createBasketDetailRequest);
            if(check) return Ok();
            else return BadRequest();
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BasketDetail>> PutBasketDetail([FromBody] UpdateBasketDetailRequest updateBasketDetailRequest)
        {
            var check = await _basketDetailService.UpdateBasketDetail(updateBasketDetailRequest);
            if (check) return Ok();
            else return BadRequest();
        }

        //// DELETE: api/BasketDetails/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteBasketDetail([FromRoute] Guid id)
        //{
        //    var basketDetail = await _basketDetailService.GetBasketDetailByID(id);
        //    if (basketDetail == null)
        //    {
        //        return NotFound();
        //    }
        //    //await _typeService.DeleteType(type);
        //    await _basketDetailService.(basketDetail.BasketId);

        //    return Ok();
        //}

    }
}
