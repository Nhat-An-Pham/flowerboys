using Microsoft.AspNetCore.Mvc;
using Models.UserDTO;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Traibanhoa.Modules.UserModule.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Traibanhoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;


        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }


        // POST api/<LoginController>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            try
            {
                var token = await _userService.GenerateToken(login);
                if (!string.IsNullOrEmpty(token))
                {
                    return new JsonResult(new
                    {
                        result = token
                    });
                }
                return NotFound();
            }
            catch
            {
                return BadRequest();
            }

        }
        [HttpPost("login-google")]
        public async Task<IActionResult> GoogleLogin([FromHeader] string authorization)
        {
            try
            {
                if (authorization == null) return BadRequest();

                string token = authorization.Split(" ")[1];
                if (!string.IsNullOrEmpty(token))
                {


                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(token);

                    var tokenS = jsonToken as JwtSecurityToken;

                    var displayName = tokenS.Claims.First(claim => claim.Type == "name").Value;
                    var email = tokenS.Claims.First(claim => claim.Type == "email").Value;
                    var avatar = tokenS.Claims.First(claim => claim.Type == "picture").Value;

                    var loginGoogle = new LoginGoogleDTO
                    {

                        Email = email,
                        Displayname = displayName,
                        Avatar = avatar
                    };


                    var accessToken = await _userService.GenerateGoolgleToken(loginGoogle);
                    return new JsonResult(new
                    {

                        result = accessToken,
                    });
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO register)
        {

            try
            {
                await _userService.Register(register);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Register successful");
        }
    }
}





