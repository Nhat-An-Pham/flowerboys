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
    public class RequestBasketDetailsController : ControllerBase
    {
        
        private readonly TraibanhoaContext _context;
        public IRepository<RequestBasketDetail> _requestBasketDetailRepo;

        public RequestBasketDetailsController(TraibanhoaContext context)
        {
            _context = context;
            _requestBasketDetailRepo = new Repository<RequestBasketDetail>(_context);
        }

        // GET: api/RequestBasketDetails
        [HttpGet]
        public async Task<IEnumerable<RequestBasketDetail>> GetRequestBasketDetails()
        {
            return _requestBasketDetailRepo.GetAll();
        }

        // GET: api/RequestBasketDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestBasketDetail>> GetRequestBasketDetail(Guid id)
        {
            var requestBasketDetail = await _requestBasketDetailRepo.GetFirstOrDefaultAsync(p => p.RequestBasketId == id);

            if (requestBasketDetail == null)
            {
                return NotFound();
            }

            return requestBasketDetail;
        }

        // PUT: api/RequestBasketDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequestBasketDetail(Guid id, RequestBasketDetail requestBasketDetail)
        {
            if (id != requestBasketDetail.RequestBasketId)
            {
                return BadRequest();
            }

            

            try
            {
                _requestBasketDetailRepo.UpdateAsync(requestBasketDetail);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestBasketDetailExists(id))
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

        // POST: api/RequestBasketDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RequestBasketDetail>> PostRequestBasketDetail(RequestBasketDetail requestBasketDetail)
        {
            
            try
            {
                _requestBasketDetailRepo.Add(requestBasketDetail);
            }
            catch (DbUpdateException)
            {
                if (RequestBasketDetailExists(requestBasketDetail.RequestBasketId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetRequestBasketDetail", new { id = requestBasketDetail.RequestBasketId }, requestBasketDetail);
        }

        // DELETE: api/RequestBasketDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequestBasketDetail(Guid id)
        {
            var requestBasketDetail = await _requestBasketDetailRepo.GetFirstOrDefaultAsync(p => p.RequestBasketId == id);
            if (requestBasketDetail == null)
            {
                return NotFound();
            }

            _requestBasketDetailRepo.RemoveAsync(requestBasketDetail);

            return NoContent();
        }

        private bool RequestBasketDetailExists(Guid id)
        {
            return _requestBasketDetailRepo.GetAll().Any(e => e.RequestBasketId == id);
        }
    }
}
