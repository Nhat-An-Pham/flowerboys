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
    public class OrderBasketDetailsController : ControllerBase
    {
        private readonly TraibanhoaContext _context;
        public IRepository<OrderBasketDetail> _orBaskeRepo;

        public OrderBasketDetailsController(TraibanhoaContext context)
        {
            _context = context;
            _orBaskeRepo = new Repository<OrderBasketDetail>(_context);
        }
        
        // GET: api/OrderBasketDetails
        [HttpGet]
        public async Task<IEnumerable<OrderBasketDetail>> GetOrderBasketDetails()
        {
            return _orBaskeRepo.GetAll();
        }

        // GET: api/OrderBasketDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderBasketDetail>> GetOrderBasketDetail(Guid id)
        {
            var orderBasketDetail = await _orBaskeRepo.GetFirstOrDefaultAsync(p => p.OrderId == id);

            if (orderBasketDetail == null)
            {
                return NotFound();
            }

            return orderBasketDetail;
        }

        // PUT: api/OrderBasketDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderBasketDetail(Guid id, OrderBasketDetail orderBasketDetail)
        {
            if (id != orderBasketDetail.OrderId)
            {
                return BadRequest();
            }

            

            try
            {
                _orBaskeRepo.UpdateAsync(orderBasketDetail);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderBasketDetailExists(id))
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

        // POST: api/OrderBasketDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderBasketDetail>> PostOrderBasketDetail(OrderBasketDetail orderBasketDetail)
        {
            
            try
            {
                _orBaskeRepo.Add(orderBasketDetail);
            }
            catch (DbUpdateException)
            {
                if (OrderBasketDetailExists(orderBasketDetail.OrderId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetOrderBasketDetail", new { id = orderBasketDetail.OrderId }, orderBasketDetail);
        }

        // DELETE: api/OrderBasketDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderBasketDetail(Guid id)
        {
            var orderBasketDetail = await _orBaskeRepo.GetFirstOrDefaultAsync(p => p.OrderId == id);
            if (orderBasketDetail == null)
            {
                return NotFound();
            }

            _orBaskeRepo.RemoveAsync(orderBasketDetail);

            return NoContent();
        }

        private bool OrderBasketDetailExists(Guid id)
        {
            return _orBaskeRepo.GetAll().Any(e => e.OrderId == id);
        }
    }
}
