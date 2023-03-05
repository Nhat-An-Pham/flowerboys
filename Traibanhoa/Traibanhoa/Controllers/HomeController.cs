using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Traibanhoa.Modules.BasketModule.Interface;
using Traibanhoa.Modules.ProductModule.Interface;

namespace Traibanhoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public HomeController(IBasketService basketService)
        {
            _basketService = basketService;
        }


        // GET: api/<ValuesController>
        [HttpGet("new-baskets")]
        public async Task<ActionResult<ICollection<Basket>>> GetNewBaskets()
        {
            try
            {
                var response = await _basketService.GetNewBasketsForHome();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/<ValuesController>
        [HttpGet("most-view")]
        public async Task<ActionResult<ICollection<Product>>> GetMostViewBaskets()
        {
            try
            {
                var response = await _basketService.GetMostViewBaskets();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/<ValuesController>
        [HttpGet("basket-price")]
        public async Task<ActionResult<ICollection<Product>>> GetBasketsByPrice()
        {
            try
            {
                var response = await _basketService.GetBasketsByPrice();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
