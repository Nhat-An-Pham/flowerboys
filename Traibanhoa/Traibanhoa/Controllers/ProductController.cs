using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Traibanhoa.Modules.ProductModule.Interface;
using Traibanhoa.Modules.ProductModule.Request;

namespace Traibanhoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsForStaff()
        {
            try
            {
                var response = await _productService.GetAll();
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }
        
        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsForCustomer()
        {
            try
            {
                var response = await _productService.GetProductsForCustomer();
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct([FromRoute] Guid id)
        {
            try
            {
                return Ok(await _productService.GetProductByID(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<ActionResult<Product>> CreateNewProduct([FromBody] CreateProductRequest createProductRequest)
        {
            try
            {
                return Ok(await _productService.AddNewProduct(createProductRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // PUT api/<ValuesController>/5
        [HttpPut]
        public async Task<IActionResult> PutProduct([FromBody] UpdateProductRequest productRequest)
        {
            try
            {
                await _productService.UpdateProduct(productRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUpdate([FromRoute] Guid id)
        {
            try
            {
                await _productService.DeleteProduct(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
