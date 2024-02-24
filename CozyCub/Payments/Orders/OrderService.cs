using CozyCub.Models.Orders.DTOs;
using Razorpay.Api;
using CozyCub.Models.Orders;
using Microsoft.EntityFrameworkCore;
using CozyCub.Models.Orders.DTOs;
using CozyCub.Models.CartModels.DTOs;
using AutoMapper;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;
using CozyCub.JWT_Id;
using Microsoft.Identity.Client;

namespace CozyCub.Payments.Orders
{
    /// <summary>
    /// Service for managing orders and payments.
    /// </summary>
    public class OrderService : IOrderService
    {

        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        private readonly string _hostUrl;
        private readonly IJwtService _jwtService;


        /// <summary>
        /// Initializes a new instance of the <see cref="OrderService"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="dbContext">The database context.</param>
        public OrderService(IConfiguration cofn, ApplicationDbContext dbContext, IJwtService jwtService)
        {
            _configuration = cofn;
            _context = dbContext;
            _hostUrl = _configuration["HostUrl:url"];
            _jwtService = jwtService;

        }

        /// <summary>
        /// Creates an order with the specified price.
        /// </summary>
        /// <param name="price">The price of the order.</param>
        /// <returns>The ID of the created order.</returns>
        public Task<string> RazorPayPayment(long price)
        {
            try
            {
                Dictionary<string, object> input = [];
                Guid transactionid = Guid.NewGuid();

                string TransactionId = transactionid.ToString();
                input.Add("Amount", Convert.ToDecimal(price) * 100);
                input.Add("Currency", "INR");
                input.Add("Receipt", TransactionId);

                string? Key = _configuration["RazorPay:KeyId"];
                string? secret = _configuration["RazorPay:KeySecret"];

                RazorpayClient client = new(Key, secret);

                Razorpay.Api.Order order = client.Order.Create(input);
                var orderId = order["id"].ToString();

                return orderId;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
                throw;
            }
        }

        /// <summary>
        /// Creates an order for the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user placing the order.</param>
        /// <param name="orderRequestDTO">The order details.</param>
        /// <returns><c>true</c> if the order was created successfully; otherwise, <c>false</c>.</returns>
        public async Task<bool> CreateOrder(string token, OrderRequestDTO orderRequestDTO)
        {
            try
            {
                int userId = _jwtService.GetUserIdFromToken(token);

                if (userId == 0)
                {
                    throw new Exception($"User id not valid with token {token}");
                }

                if (orderRequestDTO.TransactionId == null && orderRequestDTO.OrderString == null)
                {
                    return false;
                }

                var cart = await _context.Cart.Include(c => c.CartItems).ThenInclude(ci => ci.product).FirstOrDefaultAsync(u => u.UserId == userId);

                var order = new Models.Orders.Order
                {
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    CustomerEmail = orderRequestDTO.CustomerEmail,
                    CustomerName = orderRequestDTO.CustomerName,
                    CustomerPhone = orderRequestDTO.CustomerPhone,
                    CustomerCity = orderRequestDTO.CustomerCity,
                    Address = orderRequestDTO.Address,
                    OrderStatus = orderRequestDTO.OrderStatus,
                    OrderString = orderRequestDTO.OrderString,
                    TransactionId = orderRequestDTO.TransactionId,
                    OrderItems = cart.CartItems.Select(oc => new OrderedItem
                    {
                        ProductId = oc.ProductId,
                        Quantity = oc.Quantity,
                        TotalPrice = oc.Quantity * oc.product.Price

                    }).ToList()
                };

                _context.Orders.Add(order);
                _context.Cart.Remove(cart);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return false;
                throw;

            }
        }

        /// <summary>
        /// Gets the all orders for a user.
        /// </summary>
        /// <returns>The total .</returns>
        public async Task<List<OrderViewDTO>> GetOrders(int userId)
        {
            try
            {
                var orders = await _context.Orders
                    .Where(oi => oi.UserId == userId)
                    .Include(o => o.OrderItems)
                    .ToListAsync();

                if (orders.Count > 0)
                {
                    return _mapper.Map<List<OrderViewDTO>>(orders);
                }

                return new List<OrderViewDTO>();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"error occurred while fetching orders for user ID {userId}: {ex.Message}");
                return new List<OrderViewDTO>();

            }
        }

        /// <summary>
        /// Gets the total revenue from all orders.
        /// </summary>
        /// <returns>The total revenue.</returns>
        public async Task<decimal> GetTotalRevenue()
        {
            try
            {
                var order = await _context.Orders.Include(o => o.OrderItems).ToListAsync();

                if (order != null)
                {
                    var orderedItems = order.SelectMany(o => o.OrderItems);
                    var totalIncome = orderedItems.Sum(od => od.TotalPrice);

                    return totalIncome;
                }
                return 0;

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                throw;
            }

        }


        public async Task<List<OrderViewDTO>> GetOrderDetails(string token)
        {
            try
            {

                int userId = _jwtService.GetUserIdFromToken(token);


                if (userId == 0) throw new Exception($"User is not valid with token {token}");

                var order = await _context.Orders
                                     .Include(ordr => ordr.OrderItems)
                                     .ThenInclude(oi => oi.Product)
                                     .FirstOrDefaultAsync(p => p.UserId == userId);


                if (order != null)
                {
                    var orderdetails = order.OrderItems.Select(oi => new OrderViewDTO
                    {
                        Id = oi.Id,
                        OrderDate = order.OrderDate,
                        ProductName = oi.Product.Name,
                        Image = oi.Product.Image,
                        Quantity = oi.Quantity,
                        TotalPrice = oi.TotalPrice,
                        OrderId = order.OrderString,
                        OrderStatus = order.OrderStatus
                    }).ToList();
                    return orderdetails;
                }
                return new List<OrderViewDTO>();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("An exception occured while fetching user cart details" + ex.Message);
                return new List<OrderViewDTO>();

            }


        }

        async Task<AdminOrderOutputDTO> IOrderService.GetOrderDetailById(int orderId)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order != null)
                {
                    var orderDetails = new AdminOrderOutputDTO
                    {
                        Id = orderId,
                        CustomerEmail = order.CustomerEmail,
                        CustomerName = order.CustomerName,
                        CustomerCity = order.CustomerCity,
                        OrderStatus = order.OrderStatus,
                        CustomerPhone = order.CustomerPhone,
                        Orderstring = order.OrderString,
                        Address = order.Address,
                        TransactionId = order.TransactionId,
                        OrderDate = order.OrderDate,
                        ProductsPurchased = order.OrderItems.Select(oi => new OutputCartDTO
                        {
                            Id = oi.ProductId,
                            ProductName = oi.Product.Name,
                            Price = oi.Product.Price,
                            Quantity = oi.Quantity,
                            TotalPrice = oi.TotalPrice,
                            Image = oi.Product.Image
                        }).ToList()

                    };


                    return orderDetails;
                }

                return new AdminOrderOutputDTO();

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"An exception occured while retreving the order details wit order Id :{orderId}" + ex.Message);
                throw;
            }
        }


        public async Task<List<AdminOrderOutputDTO>> GetOrderDetailsForAdmin()
        {
            try
            {
                var orders = await _context.Orders
                    .Include(o => o.OrderItems)
                    .ToListAsync();

                if (orders != null)
                {
                    var oredrDetails = orders.Select(o => new AdminOrderOutputDTO
                    {
                        Id = o.Id,
                        CustomerName = o.CustomerName,
                        CustomerPhone = o.CustomerPhone,
                        CustomerEmail = o.CustomerEmail,
                        OrderId = o.OrderId,
                        OrderDate = o.OrderDate,
                        CustomerCity = o.CustomerCity,
                        Address = o.Address,
                        TransactionId = o.TransactionId,
                        OrderStatus = o.OrderStatus,
                    }).ToList();

                    return oredrDetails;
                }

                return new List<AdminOrderOutputDTO>();

            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occured while retriving the orderDetails for admin: " + ex.Message);
                throw;
            }
        }

        public List<OrderDetailDTO> payment(RazorPayDTO razorPayDTO)
        {
            Dictionary<string, string> attributes = [];
            attributes.Add("raz_pay_id", razorPayDTO.razrPayId);
            attributes.Add("raz_ord_id", razorPayDTO.razrOrdId);
            attributes.Add("raz_pay_sig", razorPayDTO.razpaySig);

            Utils.verifyPaymentLinkSignature(attributes);
            List<OrderDetailDTO> orderList =
            [
                new OrderDetailDTO
                {
                    TransactionId = razorPayDTO.razrPayId,
                    OrderId = razorPayDTO.razrOrdId
                }
            ];

            return orderList;
        }



        public async Task<bool> UpdateOrder(int orderId, AdminOrderOutputDTO adminOrder)
        {
            try
            {
                var order = await _context.Orders.FindAsync(orderId);

                // Check if the order exists
                if (order != null)
                {
                    // Update the order status
                    order.OrderStatus = adminOrder.OrderStatus;
                    // Save changes to the database
                    await _context.SaveChangesAsync();

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {

                await Console.Out.WriteLineAsync(ex.Message);
                return false;
                throw;
            }
        }


    }

}
