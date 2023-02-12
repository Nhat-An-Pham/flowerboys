using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Type = Models.Models.Type;
using Traibanhoa.Modules.TypeModule.Interface;
using Traibanhoa.Modules.TypeModule.Request;
using FluentValidation.Results;

namespace Traibanhoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private readonly ITypeService _typeService;

        public TypeController(ITypeService typeService)
        {
            _typeService = typeService;
        }



        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Type>>> GetTypes()
        {
            try
            {
                var response = await _typeService.GetAll();
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Type>> GetType([FromRoute] Guid id)
        {
            var type = await _typeService.GetTypeByID(id);

            if (type == null)
            {
                return NotFound();
            }

            return type;
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<ActionResult<Type>> CreateNewType([FromBody] CreateTypeRequest createTypeRequest)
        {
            return await _typeService.AddNewType(createTypeRequest);
        }


        // PUT api/<ValuesController>/5
        [HttpPut]
        public async Task<IActionResult> PutType([FromBody] UpdateTypeRequest typeRequest)
        {
            if (await _typeService.UpdateType(typeRequest) == false);

            return Ok();
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteType([FromRoute] Guid id)
        {
            var type = await _typeService.GetTypeByID(id);
            if (type == null)
            {
                return NotFound();
            }
            await _typeService.DeleteType(type);

            return Ok();
        }
    }
}
