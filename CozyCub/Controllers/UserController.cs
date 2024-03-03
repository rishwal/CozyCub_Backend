using CozyCub.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace CozyCub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        // Constructor to initialize dependencies
        public UserController(IConfiguration configuration, IUserService service)
        {
            _configuration = configuration;
            _userService = service;
        }

        // Get all users (only accessible by admin)
        [HttpGet]
        [Authorize(Roles = "admin")] // Requires admin role
        [ProducesResponseType(typeof(object), 200)] // Successful response
        [ProducesResponseType(401)] // Unauthorized response
        [ProducesResponseType(500)] // Server error response
        public async Task<ActionResult> GetUsers()
        {
            try
            {
                // Retrieve all users
                return Ok(await _userService.GetUsers());
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return StatusCode(500, ex.Message);
            }
        }

        // Get user by ID (only accessible by admin)
        [HttpGet("{id}")]
        [Authorize(Roles = "admin")] // Requires admin role
        [ProducesResponseType(typeof(object), 200)] // Successful response
        [ProducesResponseType(401)] // Unauthorized response
        [ProducesResponseType(500)] // Server error response
        public async Task<ActionResult> GetUserById(int id)
        {
            try
            {
                // Retrieve user by ID
                return Ok(await _userService.GetUserById(id));
            }
            catch (Exception e)
            {
                // Return server error if an exception occurs
                return StatusCode(500, e.Message);
            }
        }

        // Block a user
        [HttpPut("block-user")]
        [ProducesResponseType(200)] // Successful response
        [ProducesResponseType(400)] // Bad request response
        [ProducesResponseType(404)] // Not found response
        public async Task<ActionResult> BlockUser(int userId)
        {
            try
            {
                // Check if userId is valid
                if (userId <= 0)
                {
                    return BadRequest();
                }

                // Attempt to block the user
                var status = await _userService.BanUser(userId);
                if (!status)
                {
                    return NotFound("User not found");
                }
                return Ok();
            }
            catch (Exception e)
            {
                // Return bad request if an exception occurs
                return BadRequest(e.Message);
            }
        }

        // Unblock a user
        [HttpPut("unblock-user")]
        [ProducesResponseType(200)] // Successful response
        [ProducesResponseType(400)] // Bad request response
        [ProducesResponseType(404)] // Not found response
        public async Task<ActionResult> UnBlockUser(int userId)
        {
            try
            {
                // Check if userId is valid
                if (userId <= 0)
                {
                    return BadRequest();
                }

                // Attempt to unblock the user
                var status = await _userService.UnBanUser(userId);
                if (!status)
                {
                    return NotFound("User not found");
                }
                return Ok();
            }
            catch (Exception e)
            {
                // Return bad request if an exception occurs
                return BadRequest(e.Message);
            }
        }
    }
}
