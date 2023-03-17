using Microsoft.AspNetCore.Mvc;
using Models.Constant;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Traibanhoa.Modules.RequestBasketModule.Interface;
using Traibanhoa.Modules.RequestBasketModule.Request;
using Traibanhoa.Modules.UserModule.Interface;

namespace Traibanhoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestBasketsController : ControllerBase
    {
        private readonly IRequestBasketService _requestBasketService;
        private readonly IUserService _userService;

        public RequestBasketsController(IRequestBasketService requestBasketService, IUserService userService)
        {
            _requestBasketService = requestBasketService;
            _userService = userService;
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

        // GET api/<ValuesController>/5
        [HttpGet("confirmer/{id}")]
        public async Task<ActionResult<RequestBasket>> GetRequestBasketByConfirmer([FromRoute] Guid id)
        {
            try
            {
                return Ok(await _requestBasketService.GetRequestBasketByConfirmerID(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("author/{id}")]
        public async Task<ActionResult<RequestBasket>> GetRequestBasketByAuthor([FromRoute] Guid id)
        {
            try
            {
                return Ok(await _requestBasketService.GetRequestBasketByAuthorID(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Post api/<ValuesController>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RequestBasket>> PostRequestBasket()
        {
            try
            {
                var currentCustomer = _userService.GetCurrentUser(Request.Headers["Authorization"]);
                if (currentCustomer == null)
                {
                    throw new Exception(ErrorMessage.UserError.USER_NOT_LOGIN);
                }
                return Ok(await _requestBasketService.AddNewRequestBasket(currentCustomer.Id));
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

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequestBasket([FromRoute] Guid id)
        {
            try
            {
                await _requestBasketService.DeleteRequestBasket(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
