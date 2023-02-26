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
            try
            {
                return Ok(await _requestBasketService.GetRequestBasketByID(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Post api/<ValuesController>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RequestBasket>> PostRequestBasket([FromBody] CreateRequestBasketRequest createRequestBasketRequest)
        {
            try
            {
                return Ok(await _requestBasketService.AddNewRequestBasket(createRequestBasketRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutRequestBasket([FromBody] UpdateRequestBasketRequest updateRequestBasketRequest)
        {
            try
            {
                await _requestBasketService.UpdateRequestBasket(updateRequestBasketRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
