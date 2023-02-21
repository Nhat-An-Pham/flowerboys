using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Traibanhoa.Modules.RequestBasketDetailModule.Interface;
using Traibanhoa.Modules.RequestBasketDetailModule.Request;
using Traibanhoa.Modules.TypeModule.Request;

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

        // GET api/<ValuesController>
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

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestBasketDetail>> GetRequestBasketDetail([FromRoute] Guid id)
        {
            try
            {
                return Ok(await _requestBasketDetailService.GetRequestBasketDetailByID(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<ValuesController>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RequestBasketDetail>> PostRequestBasketDetail([FromBody] CreateRequestBasketDetailRequest createRequestBasketDetailRequest)
        {
            try
            {
                return Ok(await _requestBasketDetailService.AddNewRequestBasketDetail(createRequestBasketDetailRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PotRequestBasketDetail([FromBody] UpdateRequestBasketDetailRequest updateRequestBasketDetailRequest)
        {
            try
            {
                await _requestBasketDetailService.UpdateRequestBasketDetail(updateRequestBasketDetailRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/RequestBasketDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequestBasketDetail(Guid id)
        {
            try
            {
                await _requestBasketDetailService.DeleteRequestBasketDetail(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
