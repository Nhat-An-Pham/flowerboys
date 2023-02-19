using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Traibanhoa.Modules.CustomerModule.Interface;
using Traibanhoa.Modules.CustomerModule.Request;

namespace Traibanhoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            try
            {
                var response = await _customerService.GetAll();
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer([FromRoute]Guid id)
        {
            var customer = await _customerService.GetCustomerByID(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // POST api/<ValuesController>
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PostCustomer([FromBody] CreateCustomerRequest createCustomerRequest)
        {
            var check = await _customerService.AddNewCustomer(createCustomerRequest);
            if (check) return Ok();
            else return BadRequest();
        }

        // PUT api/<ValuesController>/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PutCustomer([FromBody] UpdateCustomerRequest updateCustomerRequest)
        {
            var check = await _customerService.UpdateCustomer(updateCustomerRequest);
            if (check) return Ok();
            else return BadRequest();
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var customer = await _customerService.GetCustomerByID(id);
            if (customer == null)
            {
                return NotFound();
            }

            var check = await _customerService.DeleteCustomer(customer);

            if (check) return Ok();
            else return BadRequest();
        }

    }
}
