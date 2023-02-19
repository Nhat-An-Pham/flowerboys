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
        public async Task<ActionResult<IEnumerable<Type>>> GetTypesForCustomer()
        {
            try
            {
                var response = await _typeService.GetTypesForCustomer();
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET: api/<ValuesController>
        [HttpGet("staff-manage")]
        public async Task<ActionResult<IEnumerable<Type>>> GetTypesForStaff()
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

        // GET: api/<ValuesController>
        [HttpGet("dropdown")]
        public async Task<ActionResult<IEnumerable<Type>>> GetTypesDropDown()
        {
            try
            {
                var response = await _typeService.GetTypeDropdown();
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
            try
            {
                return Ok(await _typeService.GetTypeByID(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<ActionResult<Type>> CreateNewType([FromBody] CreateTypeRequest createTypeRequest)
        {
            try
            {
                return Ok(await _typeService.AddNewType(createTypeRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // PUT api/<ValuesController>/5
        [HttpPut]
        public async Task<IActionResult> PutType([FromBody] UpdateTypeRequest typeRequest)
        {
            try
            {
                await _typeService.UpdateType(typeRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteType([FromRoute] Guid id)
        {

            try
            {
                await _typeService.DeleteType(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
