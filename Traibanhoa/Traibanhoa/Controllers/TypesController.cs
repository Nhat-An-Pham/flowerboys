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
    public class TypesController : ControllerBase
    {
        private readonly TraibanhoaContext _context;
        public IRepository<Traibanhoa.Models.Type> _typeRepo;


        public TypesController(TraibanhoaContext context)
        {
            _context = context;
            _typeRepo = new Repository<Traibanhoa.Models.Type>(_context);

        }


        // GET: api/Types
        [HttpGet]
        public async Task<IEnumerable<Traibanhoa.Models.Type>> GetTypes()
        {
            return _typeRepo.GetAll();
        }

        // GET: api/Types/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Traibanhoa.Models.Type>> GetType(Guid id)
        {
            var @type = await _typeRepo.GetFirstOrDefaultAsync(p => p.TypeId == id);

            if (@type == null)
            {
                return NotFound();
            }

            return @type;
        }

        // PUT: api/Types/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutType(Guid id, Traibanhoa.Models.Type @type)
        {
            if (id != @type.TypeId)
            {
                return BadRequest();
            }

            

            try
            {
                _typeRepo.UpdateAsync(@type);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeExists(id))
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

        // POST: api/Types
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Traibanhoa.Models.Type>> PostType(Traibanhoa.Models.Type @type)
        {
            try
            {
                _typeRepo.Add(@type);
            }
            catch (DbUpdateException)
            {
                if (TypeExists(@type.TypeId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetType", new { id = @type.TypeId }, @type);
        }

        // DELETE: api/Types/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteType(Guid id)
        {
            var @type = await _typeRepo.GetFirstOrDefaultAsync(p => p.TypeId == id);
            if (@type == null)
            {
                return NotFound();
            }

            _typeRepo.RemoveAsync(@type);

            return NoContent();
        }

        private bool TypeExists(Guid id)
        {
            return _typeRepo.GetAll().Any(e => e.TypeId == id);
        }
    }
}
