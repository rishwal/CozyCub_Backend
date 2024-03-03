using CozyCub.Models.Orders.DTOs;
using Razorpay.Api;
using CozyCub.Models.Orders;
using Microsoft.EntityFrameworkCore;
using CozyCub.Models.CartModels.DTOs;
using AutoMapper;
using CozyCub.JWT_Id;


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
        /// Creates an order for the specified user with the provided order details.
        /// </summary>
        /// <param name="token">The authentication token of the user placing the order.</param>
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
                    Address = orderRequestDTO.HomeAddress,
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
        /// Retrieves the orders associated with the specified user ID.
        /// </summary>
        /// <param name="userId">The ID of the user whose orders are to be retrieved.</param>
        /// <returns>A list of OrderViewDTO objects containing order details.</returns>
        /// <remarks>
        /// This method fetches orders from the database that are associated with the provided user ID.
        /// It includes details such as order items and maps the retrieved orders to OrderViewDTO objects.
        /// If orders are found for the user, a list of OrderViewDTO objects is returned; otherwise,
        /// an empty list is returned.
        /// </remarks>
        /// <param name="userId">The ID of the user whose orders are to be retrieved.</param>
        /// <returns>A list of OrderViewDTO objects containing order details.</returns>
        /// <exception cref="Exception">Thrown when an error occurs while fetching orders.</exception>
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

        /// <summary>
        /// Retrieves the order details for the user associated with the provided authentication token.
        /// </summary>
        /// <param name="token">The authentication token of the user.</param>
        /// <returns>A list of OrderViewDTO objects containing order details.</returns>
        /// <remarks>
        /// This method fetches the order details for the user identified by the provided token.
        /// It retrieves orders from the database, including associated order items and product information,
        /// and constructs a list of OrderViewDTO objects containing relevant order details.
        /// If the user is not valid or an exception occurs during the process, an empty list is returned.
        /// </remarks>
        public async Task<List<OrderViewDTO>> GetOrderDetails(string token)
        {
            try
            {

                int userId = _jwtService.GetUserIdFromToken(token);


                if (userId == 0) throw new Exception($"User is not valid with token {token}");

                var orders = await _context.Orders
                                     .Include(ordr => ordr.OrderItems)
                                     .ThenInclude(oi => oi.Product)
                                     .Where(o => o.UserId == userId)
                                     .ToListAsync();



                var orderDetails = new List<OrderViewDTO>();

                foreach (var order in orders)
                {
                    foreach (var orderItem in order.OrderItems)
                    {
                        var orderDetail = new OrderViewDTO
                        {
                            Id = orderItem.Id,
                            OrderDate = order.OrderDate,
                            ProductName = orderItem.Product.Name,
                            Image = _hostUrl + orderItem.Product.Image,
                            Quantity = orderItem.Quantity,
                            TotalPrice = orderItem.TotalPrice,
                            OrderId = order.OrderString,
                            OrderStatus = order.OrderStatus
                        };

                        orderDetails.Add(orderDetail);
                    }
                }

                return orderDetails;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("An exception occured while fetching user cart details" + ex.Message);
                return new List<OrderViewDTO>();

            }


        }


        /// <summary>
        /// Retrieves the order details for the specified order ID.
        /// </summary>
        /// <param name="orderId">The ID of the order to retrieve.</param>
        /// <returns>An AdminOrderOutputDTO object containing the order details.</returns>
        /// <remarks>
        /// This method fetches the details of the order identified by the provided order ID from the database.
        /// It includes information such as customer details, order date, address, transaction ID, order status,
        /// and the products purchased within the order, along with their details.
        /// If the order is found, an AdminOrderOutputDTO object containing the details is returned; otherwise,
        /// an empty AdminOrderOutputDTO object is returned.
        /// </remarks>
        /// <param name="orderId">The ID of the order to retrieve.</param>
        /// <returns>An AdminOrderOutputDTO object containing the order details.</returns>
        /// <exception cref="Exception">Thrown when an error occurs while retrieving order details.</exception>
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
                throw new Exception("An exception occured while retreving the order details wit order Id : { orderId}" + ex.Message);
            }
        }

        /// <summary>
        /// Retrieves order details for administrative purposes.
        /// </summary>
        /// <returns>A list of AdminOrderOutputDTO objects containing order details.</returns>
        /// <remarks>
        /// This method fetches order details including associated order items from the database for administrative use.
        /// It constructs a list of AdminOrderOutputDTO objects containing relevant order information such as customer details,
        /// order date, address, transaction ID, and order status.
        /// If no orders are found, an empty list is returned.
        /// </remarks>
        /// <exception cref="Exception">Thrown when an error occurs while retrieving order details.</exception>

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


        /// <summary>
        /// Processes a payment using the provided RazorPayDTO.
        /// </summary>
        /// <param name="razorPayDTO">The RazorPayDTO containing payment details.</param>
        /// <returns><c>true</c> if the payment processing is successful; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method verifies the presence of required parameters in the RazorPayDTO and initializes
        /// a RazorpayClient using the Razorpay API keys obtained from the application configuration.
        /// It constructs a dictionary of attributes required for payment verification and calls the 
        /// utility method Utils.verifyPaymentLinkSignature to verify the payment signature.
        /// If all necessary parameters are present and the payment signature is successfully verified, 
        /// the method returns <c>true</c>; otherwise, it returns <c>false</c>.
        /// </remarks>

        public bool payment(RazorPayDTO razorPayDTO)
        {
            if (razorPayDTO == null ||
               razorPayDTO.razrPayId == null ||
               razorPayDTO.razrOrdId == null ||
               razorPayDTO.razpaySig == null
               )
                return false;
            RazorpayClient client = new RazorpayClient(_configuration["Razorpay:keyId"], _configuration["Razorpay:keySecret"]);
            Dictionary<string, string> attributes = [];
            attributes.Add("razorpay_payment_id", razorPayDTO.razrPayId);
            attributes.Add("razorpay_order_id", razorPayDTO.razrOrdId);
            attributes.Add("razorpay_signature", razorPayDTO.razpaySig);

            Utils.verifyPaymentLinkSignature(attributes);

            return true;


        }


        /// <summary>
        /// Updates the status of the specified order.
        /// </summary>
        /// <param name="orderId">The ID of the order to update.</param>
        /// <param name="adminOrder">The AdminOrderOutputDTO containing the updated order status.</param>
        /// <returns><c>true</c> if the order status was updated successfully; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method retrieves the order from the database based on the provided order ID,
        /// updates its status with the value from the provided AdminOrderOutputDTO, and saves
        /// the changes to the database. If the order is found and the status is updated successfully,
        /// it returns <c>true</c>; otherwise, it returns <c>false</c>.
        /// </remarks>
        /// <exception cref="Exception">Thrown when an error occurs while updating the order status.</exception>

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
                throw new Exception(ex.Message);
            }
        }


    }

}
