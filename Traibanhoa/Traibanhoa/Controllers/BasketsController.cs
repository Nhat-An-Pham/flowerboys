using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Traibanhoa.Modules.BasketModule.Interface;
using Traibanhoa.Modules.BasketModule.Request;

namespace Traibanhoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketsController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        // GET api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Basket>>> GetBaskets()
        {
            try
            {
                var response = await _basketService.GetAll();
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Basket>> GetBasket([FromRoute] Guid id)
        {
            var basket = await _basketService.GetBasketByID(id);

            if (basket == null)
            {
                return NotFound();
            }

            return basket;
        }

        // POST api/<ValuesController>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PostBasket([FromBody] CreateBasketRequest createBasketRequest)
        {
            var check = await _basketService.AddNewBasket(createBasketRequest);
            if (check) return Ok();
            else return BadRequest();
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Basket>> PutBasket([FromBody] UpdateBasketRequest updateBasketRequest)
        {
            var check = await _basketService.UpdateBasket(updateBasketRequest);
            if (check) return Ok();
            else return BadRequest();
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasket([FromRoute] Guid id)
        {
            var basket = await _basketService.GetBasketByID(id);
            if (basket == null)
            {
                return NotFound();
            }
            var check = await _basketService.DeleteBasket(basket);

            if (check) return Ok();
            else return BadRequest();
        }
    }
}
