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
    public class OrderProductDetailsController : ControllerBase
    {

        private readonly TraibanhoaContext _context;
        public IRepository<OrderProductDetail> _orProductDetailRepo;

        public OrderProductDetailsController(TraibanhoaContext context)
        {
            _context = context;
            _orProductDetailRepo = new Repository<OrderProductDetail>(_context);
        }

        // GET: api/OrderProductDetails
        [HttpGet]
        public async Task<IEnumerable<OrderProductDetail>> GetOrderProductDetails()
        {
            return _orProductDetailRepo.GetAll();
        }

        // GET: api/OrderProductDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderProductDetail>> GetOrderProductDetail(Guid id)
        {
            var orderProductDetail = await _orProductDetailRepo.GetFirstOrDefaultAsync(p => p.ProductId == id);

            if (orderProductDetail == null)
            {
                return NotFound();
            }

            return orderProductDetail;
        }

        // PUT: api/OrderProductDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderProductDetail(Guid id, OrderProductDetail orderProductDetail)
        {
            if (id != orderProductDetail.OrderId)
            {
                return BadRequest();
            }

            

            try
            {
                _orProductDetailRepo.UpdateAsync(orderProductDetail);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderProductDetailExists(id))
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

        // POST: api/OrderProductDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderProductDetail>> PostOrderProductDetail(OrderProductDetail orderProductDetail)
        {
            
            try
            {
                _orProductDetailRepo.Add(orderProductDetail);
            }
            catch (DbUpdateException)
            {
                if (OrderProductDetailExists(orderProductDetail.OrderId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetOrderProductDetail", new { id = orderProductDetail.OrderId }, orderProductDetail);
        }

        // DELETE: api/OrderProductDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderProductDetail(Guid id)
        {
            var orderProductDetail = await _orProductDetailRepo.GetFirstOrDefaultAsync(p => p.ProductId == id);
            if (orderProductDetail == null)
            {
                return NotFound();
            }

            _orProductDetailRepo.RemoveAsync(orderProductDetail);

            return NoContent();
        }

        private bool OrderProductDetailExists(Guid id)
        {
            return _orProductDetailRepo.GetAll().Any(e => e.OrderId == id);
        }
    }
}
