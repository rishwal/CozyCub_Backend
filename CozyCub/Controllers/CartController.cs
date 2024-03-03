using CozyCub.JWT_Id;
using CozyCub.Services.CartServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CozyCub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartServices _cartServices;
        private readonly IConfiguration _configuration;

        // Constructor to initialize dependencies
        public CartController(ICartServices cartServices, IConfiguration configuration)
        {
            _configuration = configuration;
            _cartServices = cartServices;
        }

        // Get cart items for the authenticated user
        [HttpGet("GetCartItems")]
        [Authorize] // Requires authentication
        [ProducesResponseType(typeof(object), 200)] // Response type when successful
        [ProducesResponseType(401)] // Unauthorized response
        [ProducesResponseType(500)] // Server error response
        public async Task<ActionResult> GetCartItems()
        {
            try
            {
                // Extract JWT token from request header
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var splitToken = token.Split(' ');
                var jwtToken = splitToken[1];

                // Get cart items using the JWT token
                return Ok(await _cartServices.GetCartItems(jwtToken));
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return StatusCode(500, ex.Message);
            }
        }

        // Add a product to the cart for the authenticated user
        [HttpPost("add-to-cart")]
        [Authorize] // Requires authentication
        [ProducesResponseType(200)] // Successful response
        [ProducesResponseType(500)] // Server error response
        public async Task<ActionResult> AddToCart(int productId)
        {
            try
            {
                // Extract JWT token from request header
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var splitToken = token.Split(' ');
                var jwtToken = splitToken[1];

                // Add product to the cart using the JWT token and product ID
                var isok = await _cartServices.AddToCart(jwtToken, productId);
                return Ok(isok);
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return StatusCode(500, ex.Message);
            }
        }

        // Increment quantity of a product in the cart
        [HttpPut("increment-quantity")]
        [ProducesResponseType(200)] // Successful response
        [ProducesResponseType(500)] // Server error response
        public async Task<IActionResult> IncrementQuantity(int productId)
        {
            try
            {
                // Extract JWT token from request header
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var splitToken = token.Split(' ');
                var jwtToken = splitToken[1];

                // Increment quantity of the product in the cart
                bool res = await _cartServices.IncreaseQuantity(jwtToken, productId);
                return res ? Ok() : StatusCode(500, "An error occurred while incrementing quantity!");
            }
            catch (Exception e)
            {
                // Return server error if an exception occurs
                return StatusCode(500, e.Message);
            }
        }

        // Decrement quantity of a product in the cart
        [HttpPut("decrement-quantity")]
        [ProducesResponseType(200)] // Successful response
        [ProducesResponseType(500)] // Server error response
        public async Task<IActionResult> DecrementQuantity(int productId)
        {
            try
            {

                // Extract JWT token from request header
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var splitToken = token.Split(' ');
                var jwtToken = splitToken[1];

                // Decrement quantity of the product in the cart
                await _cartServices.DecreaseQuantity(jwtToken, productId);
                return Ok();
            }
            catch (Exception e)
            {
                // Return server error if an exception occurs
                return StatusCode(500, e.Message);
            }
        }

        // Remove a product from the cart
        [HttpDelete("remove-item-from-cart")]
        [ProducesResponseType(typeof(bool), 200)] // Successful response
        [ProducesResponseType(500)] // Server error response
        public async Task<ActionResult> RemoveCartItem(int productId)
        {
            try
            {
                // Extract JWT token from request header
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var splitToken = token.Split(' ');
                var jwtToken = splitToken[1];

                // Remove the product from the cart
                bool res = await _cartServices.DeleteFromCart(jwtToken, productId);
                return Ok(res);
            }
            catch (Exception e)
            {
                // Return server error if an exception occurs
                return StatusCode(500, e.Message);
            }
        }
    }
}
