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

        public OrderController(IOrderService orderServices)
        {
            _orderServices = orderServices;
        }

        [HttpPost("razor-payment")]
        [Authorize]
        [ProducesResponseType(typeof(long), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> CreateOrder(long price)
        {
            try
            {
                if (price <= 0 || price > 100000 )
                {
                    return BadRequest("Enter a valid amount!");
                }
                var orderId = await _orderServices.RazorPayPayment(price);
                return Ok(orderId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("payment")]
        [Authorize]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public ActionResult Payment(RazorPayDTO razorpay)
        {
            try
            {
                if (razorpay == null)
                {
                    return BadRequest("Razorpay details must not be null here");
                }
                var con = _orderServices.payment(razorpay);
                return Ok(con);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("place-Order")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> PlaceOrder(int userId, OrderRequestDTO orderRequests)
        {
            try
            {
                if (orderRequests == null || userId <= 0)
                {
                    return BadRequest();
                }
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var splitToken = token.Split(' ');
                var jwtToken = splitToken[1];
                await _orderServices.CreateOrder(jwtToken, orderRequests);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("get-order-details-as-admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> GetOrderDetailsAsAdmin(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest();
                }
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var splitToken = token.Split(' ');
                var jwtToken = splitToken[1];
                return Ok(await _orderServices.GetOrderDetails(jwtToken));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("total_revenue")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> GetTotalRevenue()
        {
            try
            {
                return Ok(await _orderServices.GetTotalRevenue());
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("get-detailed-order")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> GetDetailedOrder()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var splitToken = token.Split(' ');
                var jwtToken = splitToken[1];
                return Ok(await _orderServices.GetOrderDetails(jwtToken));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("update-order-status")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdateOrder(int orderId, [FromBody] AdminOrderOutputDTO orderAdminView)
        {
            try
            {
                await _orderServices.UpdateOrder(orderId, orderAdminView);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
