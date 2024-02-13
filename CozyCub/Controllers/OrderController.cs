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

        [HttpPost("order-create")]
        [Authorize]
        [ProducesResponseType(typeof(long), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> CreateOrder(long price)
        {
            try
            {
                if (price <= 0)
                {
                    return BadRequest("Enter a valid amount!");
                }
                var orderId = await _orderServices.CreateOrder(price);
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
                await _orderServices.CreateOrder(userId, orderRequests);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("get_order_details")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> GetOrderDetails(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest();
                }
                return Ok(await _orderServices.GetOrderDetails(userId));
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
        public async Task<ActionResult> GetDetailedOrder(int orderId)
        {
            try
            {
                return Ok(await _orderServices.GetOrderDetails(orderId));
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
