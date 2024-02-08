using CozyCub.Interfaces;
using CozyCub.Models.User.DTOs;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CozyCub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;


        public AuthController(IAuthService service)
        {
            _authService = service;
        }


        [HttpPost( "Register")]
        public async Task<ActionResult<string>> Register([FromBody] UserRegisterDTO user)
        {
            try
            {
                var token = await _authService.Register(user);
                return Ok(new { Token = token });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "Registration failed.", Error = e.Message });
            }
        }


        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(UserLoginDTO user)
        {
            try
            {
                var token = _authService.Login(user);
                return Ok(new { Token = token });
            }
            catch (Exception e)
            {
                return Unauthorized(new { Mesaage = "Lohin failed.", Error = e.Message });

            }
        }





    }
}
