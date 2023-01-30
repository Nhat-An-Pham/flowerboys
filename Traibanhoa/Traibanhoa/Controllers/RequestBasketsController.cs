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
    public class RequestBasketsController : ControllerBase
    {
        private readonly TraibanhoaContext _context;
        public IRepository<RequestBasket> _requestBasketRepo;

        public RequestBasketsController(TraibanhoaContext context)
        {
            _context = context;
            _requestBasketRepo = new Repository<RequestBasket>(_context);
        }


        // GET: api/RequestBaskets
        [HttpGet]
        public async Task<IEnumerable<RequestBasket>> GetRequestBaskets()
        {
            return _requestBasketRepo.GetAll();
        }

        // GET: api/RequestBaskets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestBasket>> GetRequestBasket(Guid id)
        {
            var requestBasket = await _requestBasketRepo.GetFirstOrDefaultAsync(p => p.RequestBasketId == id);

            if (requestBasket == null)
            {
                return NotFound();
            }

            return requestBasket;
        }

        // PUT: api/RequestBaskets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequestBasket(Guid id, RequestBasket requestBasket)
        {
            if (id != requestBasket.RequestBasketId)
            {
                return BadRequest();
            }


            try
            {
                _requestBasketRepo.UpdateAsync(requestBasket);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestBasketExists(id))
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

        // POST: api/RequestBaskets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RequestBasket>> PostRequestBasket(RequestBasket requestBasket)
        {
            try
            {
                _requestBasketRepo.Add(requestBasket);
            }
            catch (DbUpdateException)
            {
                if (RequestBasketExists(requestBasket.RequestBasketId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetRequestBasket", new { id = requestBasket.RequestBasketId }, requestBasket);
        }

        // DELETE: api/RequestBaskets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequestBasket(Guid id)
        {
            var requestBasket = await _requestBasketRepo.GetFirstOrDefaultAsync(p => p.RequestBasketId == id);
            if (requestBasket == null)
            {
                return NotFound();
            }

            _requestBasketRepo.RemoveAsync(requestBasket);

            return NoContent();
        }

        private bool RequestBasketExists(Guid id)
        {
            return _requestBasketRepo.GetAll().Any(e => e.RequestBasketId == id);
        }
    }
}
