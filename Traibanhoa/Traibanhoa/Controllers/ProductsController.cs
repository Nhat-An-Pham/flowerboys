using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Utils.Repository.Interface;
using Repository.Utils.Repository;
using Traibanhoa.Models;

namespace Traibanhoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly TraibanhoaContext _context;
        public IRepository<Product> _productRepo;

        public ProductsController(TraibanhoaContext context)
        {
            _context = context;
            _productRepo = new Repository<Product>(_context);
        }

        // GET: api/Products1
        [HttpGet]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return _productRepo.GetAll();
        }

        // GET: api/Products1/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            //var product = await _context.Products.FindAsync(id);
            var product = await _productRepo.GetFirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products1/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(Guid id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            //_context.Entry(product).State = EntityState.Modified;

            try
            {
                //await _context.SaveChangesAsync();
                _productRepo.UpdateAsync(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            //_context.Products.Add(product);
            try
            {
                //await _context.SaveChangesAsync();
                _productRepo.Add(product);
            }
            catch (DbUpdateException)
            {
                if (ProductExists(product.ProductId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        // DELETE: api/Products1/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            //var product = await _context.Products.FindAsync(id);
            var product = await _productRepo.GetFirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            //_context.Products.Remove(product);
            //await _context.SaveChangesAsync();
            _productRepo.RemoveAsync(product);

            return NoContent();
        }

        private bool ProductExists(Guid id)
        {
            return _productRepo.GetAll().Any(e => e.ProductId == id);
        }
    }
}
