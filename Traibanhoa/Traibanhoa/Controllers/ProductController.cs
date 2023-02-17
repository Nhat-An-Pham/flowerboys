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
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
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

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct([FromRoute] Guid id)
        {
            var product = await _productService.GetProductByID(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<ActionResult<Product>> CreateNewProduct([FromBody] CreateProductRequest createProductRequest)
        {
            var productId = await _productService.AddNewProduct(createProductRequest);
            if (productId == null) return BadRequest();
            else return Ok(productId);
        }


        // PUT api/<ValuesController>/5
        [HttpPut]
        public async Task<IActionResult> PutProduct([FromBody] UpdateProductRequest productRequest)
        {
            if (await _productService.UpdateProduct(productRequest) == false) return BadRequest();
            else return Ok();
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUpdate([FromRoute] Guid id)
        {
            if (await _productService.DeleteProduct(id) == false) return BadRequest();
            return Ok();
        }
    }
}
