using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Traibanhoa.Modules.RequestBasketModule.Interface;
using Traibanhoa.Modules.RequestBasketModule.Request;

namespace Traibanhoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestBasketsController : ControllerBase
    {
        private readonly IRequestBasketService _requestBasketService;

        public RequestBasketsController(IRequestBasketService requestBasketService)
        {
            _requestBasketService = requestBasketService;
        }

        // GET api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestBasket>>> GetRequestBaskets()
        {
            try
            {
                var response = await _requestBasketService.GetAll();
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestBasket>> GetRequestBasket([FromRoute] Guid id)
        {
            var requestBasket = await _requestBasketService.GetRequestBasketByID(id);

            if (requestBasket == null)
            {
                return NotFound();
            }

            return requestBasket;
        }

        // Post api/<ValuesController>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PostRequestBasket([FromBody] CreateRequestBasketRequest createRequestBasketRequest)
        {
            var check = await _requestBasketService.AddNewRequestBasket(createRequestBasketRequest);
            if (check) return Ok();
            else return BadRequest();
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RequestBasket>> PutRequestBasket([FromBody] UpdateRequestBasketRequest updateRequestBasketRequest)
        {
            var check = await _requestBasketService.UpdateRequestBasket(updateRequestBasketRequest);
            if (check) return Ok();
            else return BadRequest();
        }

        //// DELETE: api/RequestBaskets/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteRequestBasket(Guid id)
        //{
        //    var requestBasket = await _context.RequestBaskets.FindAsync(id);
        //    if (requestBasket == null)
        //    {
        //        return NotFound();
        //    }
        //    _requestBasketService.De
        //    _context.RequestBaskets.Remove(requestBasket);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool RequestBasketExists(Guid id)
        //{
        //    return _context.RequestBaskets.Any(e => e.RequestBasketId == id);
        //}
    }
}
