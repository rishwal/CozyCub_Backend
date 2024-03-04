using CozyCub.Models.Orders.DTOs;
using CozyCub.Payments;
using CozyCub.Payments.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CozyCub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderServices;

        // Constructor to initialize dependencies
        public OrderController(IOrderService orderServices)
        {
            _orderServices = orderServices;
        }

        // Create an order with RazorPay payment
        [HttpPost("razor-payment")]
        [Authorize] // Requires authentication
        [ProducesResponseType(typeof(long), 200)] // Successful response
        [ProducesResponseType(typeof(string), 400)] // Bad request response
        [ProducesResponseType(401)] // Unauthorized response
        [ProducesResponseType(500)] // Server error response
        public async Task<ActionResult> CreateOrder(long price)
        {
            try
            {
                // Check if price is valid
                if (price <= 0 || price > 100000)
                {
                    return BadRequest("Enter a valid amount!");
                }
                // Create order with RazorPay payment
                var orderId = await _orderServices.RazorPayPayment(price);
                return Ok(orderId);
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return StatusCode(500, ex.Message);
            }
        }

        // Payment endpoint
        [HttpPost("payment")]
        [Authorize] // Requires authentication
        [ProducesResponseType(typeof(object), 200)] // Successful response
        [ProducesResponseType(typeof(string), 400)] // Bad request response
        [ProducesResponseType(401)] // Unauthorized response
        [ProducesResponseType(500)] // Server error response
        public ActionResult Payment(RazorPayDTO razorpay)
        {
            try
            {
                // Check if razorpay details are provided
                if (razorpay == null)
                {
                    return BadRequest("Razorpay details must not be null here");
                }
                // Process payment
                var con = _orderServices.payment(razorpay);
                return Ok(con);
            }
            catch (Exception e)
            {
                // Return server error if an exception occurs
                return StatusCode(500, e.Message);
            }
        }

        // Place an order
        [HttpPost("place-Order")]
        [Authorize] // Requires authentication
        [ProducesResponseType(200)] // Successful response
        [ProducesResponseType(400)] // Bad request response
        [ProducesResponseType(401)] // Unauthorized response
        [ProducesResponseType(500)] // Server error response
        public async Task<ActionResult> PlaceOrder(OrderRequestDTO orderRequest)
        {
            try
            {
                // Check if orderRequests and userId are valid
                if (orderRequest == null)
                {
                    return BadRequest();
                }
                // Get JWT token from request header
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var splitToken = token.Split(' ');
                var jwtToken = splitToken[1];
                // Create order
                if (orderRequest == null || jwtToken == null)
                {
                    return BadRequest();
                }
                var status = await _orderServices.CreateOrder(jwtToken, orderRequest);
                return Ok(status);
            }
            catch (Exception e)
            {
                // Return server error if an exception occurs
                return StatusCode(500, e.Message);
            }
        }

        // Get order details as an admin
        [HttpGet("get-order-details-as-admin")]
        [Authorize(Roles = "admin")] // Requires admin role
        [ProducesResponseType(typeof(object), 200)] // Successful response
        [ProducesResponseType(400)] // Bad request response
        [ProducesResponseType(401)] // Unauthorized response
        [ProducesResponseType(500)] // Server error response
        public async Task<ActionResult> GetOrderDetailsAsAdmin(int userId)
        {
            try
            {
                // Check if userId is valid
                if (userId <= 0)
                {
                    return BadRequest();
                }
                return Ok(await _orderServices.GetOrderDetailsForAdmin());
            }
            catch (Exception e)
            {
                // Return server error if an exception occurs
                return StatusCode(500, e.Message);
            }
        }

        // Get total revenue
        [HttpGet("total_revenue")]
        [Authorize(Roles = "admin")] // Requires admin role
        [ProducesResponseType(typeof(object), 200)] // Successful response
        [ProducesResponseType(401)] // Unauthorized response
        [ProducesResponseType(500)] // Server error response
        public async Task<ActionResult> GetTotalRevenue()
        {
            try
            {
                // Get total revenue
                return Ok(await _orderServices.GetTotalRevenue());
            }
            catch (Exception e)
            {
                // Return server error if an exception occurs
                return StatusCode(500, e.Message);
            }
        }

        // Get detailed order as an admin
        [HttpGet("get-detailed-order")]
        [Authorize(Roles = "admin")] // Requires admin role
        [ProducesResponseType(typeof(object), 200)] // Successful response
        [ProducesResponseType(401)] // Unauthorized response
        [ProducesResponseType(500)] // Server error response
        public async Task<ActionResult> GetDetailedOrder()
        {
            try
            {
                // Get JWT token from request header
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var splitToken = token.Split(' ');
                var jwtToken = splitToken[1];
                return Ok(await _orderServices.GetOrderDetails(jwtToken));
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return StatusCode(500, ex.Message);
            }
        }

        // Update order status as an admin
        [HttpPut("update-order-status")]
        [Authorize(Roles = "admin")] // Requires admin role
        [ProducesResponseType(200)] // Successful response
        [ProducesResponseType(401)] // Unauthorized response
        [ProducesResponseType(500)] // Server error response
        public async Task<ActionResult> UpdateOrder(int orderId, [FromBody] AdminOrderOutputDTO orderAdminView)
        {
            try
            {
                // Update order status
                await _orderServices.UpdateOrder(orderId, orderAdminView);
                return Ok();
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return StatusCode(500, ex.Message);
            }
        }
    }
}
