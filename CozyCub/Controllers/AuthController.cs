using CozyCub.Models.UserModels.DTOs;
using CozyCub.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace CozyCub.Controllers
{
    /// <summary>
    /// Controller for handling user authentication operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// Constructor for AuthController.
        /// </summary>
        /// <param name="service">The authentication service.</param>
        public AuthController(IAuthService service)
        {
            _authService = service;
        }

        /// <summary>
        /// Endpoint for user registration.
        /// </summary>
        /// <param name="user">User registration data.</param>
        /// <returns>
        /// Returns 200 OK with authentication token upon successful registration.
        /// Returns 400 BadRequest with error message upon failure.
        /// </returns>
        [HttpPost("Register")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
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

        /// <summary>
        /// Endpoint for user login.
        /// </summary>
        /// <param name="user">User login credentials.</param>
        /// <returns>
        /// Returns 200 OK with authentication token upon successful login.
        /// Returns 401 Unauthorized with error message upon failure.
        /// </returns>
        [HttpPost("Login")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 401)]
        public async Task<ActionResult<string>> Login(UserLoginDTO user)
        {
            try
            {
                var token = await _authService.Login(user);
                return Ok(new { Token = token });
            }
            catch (Exception e)
            {
                return Unauthorized(new { Message = "Login failed.", Error = e.Message });
            }
        }
    }
}
