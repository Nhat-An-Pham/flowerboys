using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Traibanhoa.Modules.UserModule.Interface;
using Traibanhoa.Modules.UserModule.Request;

namespace Traibanhoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                var response = await _userService.GetAll();
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser([FromRoute]Guid id)
        {
            var user = await _userService.GetUserByID(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromBody] CreateUserRequest createUserRequest)
        {
            var check = await _userService.AddNewUser(createUserRequest);
            if (check) return Ok();
            else return BadRequest();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromBody] UpdateUserRequest updateUserRequest)
        {
            var check = await _userService.UpdateUser(updateUserRequest);
            if (check) return Ok();
            else return BadRequest();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            var user = await _userService.GetUserByID(id);
            if (user == null)
            {
                return NotFound();
            }

            var check = await _userService.DeleteUser(user);

            if(check) return Ok();
            else return BadRequest();
        }

    }
}
