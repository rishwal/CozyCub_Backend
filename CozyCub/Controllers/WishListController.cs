using CozyCub.Services.WishList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CozyCub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly IWishListService _wishListService;

        // Constructor to initialize dependencies
        public WishListController(IWishListService wishListService)
        {
            _wishListService = wishListService;
        }

        // Get user's wish list
        [HttpGet("get-wishlist")]
        [Authorize] // Requires authentication
        [ProducesResponseType(typeof(object), 200)] // Successful response
        [ProducesResponseType(401)] // Unauthorized response
        [ProducesResponseType(500)] // Server error response
        public async Task<ActionResult> GetWishLists(int userId)
        {
            try
            {
                // Retrieve user's wish list
                return Ok(await _wishListService.GetWishList(userId));
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return StatusCode(500, ex.Message);
            }
        }

        // Add a product to user's wish list
        [HttpPost("add-wishlist")]
        [Authorize] // Requires authentication
        [ProducesResponseType(200)] // Successful response
        [ProducesResponseType(400)] // Bad request response
        [ProducesResponseType(500)] // Server error response
        public async Task<ActionResult> AddWishList(int userId, int productId)
        {
            try
            {
                // Check if the product is already in the wish list
                var isExist = await _wishListService.AddToWishList(userId, productId);
                if (!isExist)
                {
                    return BadRequest("Item already in the wish list");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return StatusCode(500, ex.Message);
            }
        }

        // Remove a product from user's wish list
        [HttpDelete("remove-wishlist")]
        [Authorize] // Requires authentication
        [ProducesResponseType(200)] // Successful response
        [ProducesResponseType(500)] // Server error response
        public async Task<ActionResult> RemoveFromWishList(int productId)
        {
            try
            {
                // Remove product from wish list
                await _wishListService.RemoveFromWishList(productId);
                return Ok();
            }
            catch (Exception e)
            {
                // Return server error if an exception occurs
                return StatusCode(500, e.Message);
            }
        }
    }
}
