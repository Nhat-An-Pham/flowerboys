using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Traibanhoa.Modules.RequestBasketDetailModule.Interface;
using Traibanhoa.Modules.RequestBasketDetailModule.Request;

namespace Traibanhoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestBasketDetailsController : ControllerBase
    {
        private readonly IRequestBasketDetailService _requestBasketDetailService;

        public RequestBasketDetailsController(IRequestBasketDetailService requestBasketDetailService)
        {
            _requestBasketDetailService = requestBasketDetailService;
        }

        // GET: api/RequestBasketDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestBasketDetail>>> GetRequestBasketDetails()
        {
            try
            {
                var response = await _requestBasketDetailService.GetAll();
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET: api/RequestBasketDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestBasketDetail>> GetRequestBasketDetail([FromRoute] Guid id)
        {
            var requestBasketDetail = await _requestBasketDetailService.GetRequestBasketDetailByID(id);

            if (requestBasketDetail == null)
            {
                return NotFound();
            }

            return requestBasketDetail;
        }

        // PUT: api/RequestBasketDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequestBasketDetail([FromBody] CreateRequestBasketDetailRequest createRequestBasketDetailRequest)
        {
            var check = await _requestBasketDetailService.AddNewRequestBasketDetail(createRequestBasketDetailRequest);
            if (check) return Ok();
            else return BadRequest();
        }

        // POST: api/RequestBasketDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RequestBasketDetail>> PostRequestBasketDetail([FromBody] UpdateRequestBasketDetailRequest updateRequestBasketDetailRequest)
        {
            var check = await _requestBasketDetailService.UpdateRequestBasketDetail(updateRequestBasketDetailRequest);
            if (check) return Ok();
            else return BadRequest();
        }

        //// DELETE: api/RequestBasketDetails/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteRequestBasketDetail(Guid id)
        //{
        //    var requestBasketDetail = await _context.RequestBasketDetails.FindAsync(id);
        //    if (requestBasketDetail == null)
        //    {
        //        return NotFound();
        //    }
        //    _requestBasketDetailService.De
        //    _context.RequestBasketDetails.Remove(requestBasketDetail);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

    }
}
