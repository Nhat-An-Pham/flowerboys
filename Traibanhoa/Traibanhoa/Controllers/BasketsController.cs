﻿using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Traibanhoa.Modules.BasketModule.Interface;
using Traibanhoa.Modules.BasketModule.Request;
using Traibanhoa.Modules.TypeModule.Request;

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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Basket>> GetBasket([FromRoute] Guid id)
        {
            try
            {
                return Ok(await _basketService.GetBasketByID(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<ValuesController>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Basket>> PostBasket([FromBody] CreateBasketRequest createBasketRequest)
        {
            try
            {
                return Ok(await _basketService.AddNewBasket(createBasketRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            };
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutBasket([FromBody] UpdateBasketRequest updateBasketRequest)
        {
            try
            {
                await _basketService.UpdateBasket(updateBasketRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            };
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasket([FromRoute] Guid id)
        {
            try
            {
                await _basketService.DeleteBasket(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
