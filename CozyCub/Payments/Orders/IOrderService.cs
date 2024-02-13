using CozyCub.Models.Orders.DTOs;
using CozyCub.Payments;

namespace CozyCub.Payments.Orders
{
    public interface IOrderService
    {
        Task<string> CreateOrder(long price);
        public List<OrderDetailDTO> payment(RazorPayDTO razorPayDTO);

        Task<bool> CreateOrder(int userId, OrderRequestDTO orderRequestDTO);

        Task<List<OrderViewDTO>> GetOrders(int userId);

        Task<decimal> GetTotalRevenue();

        Task<List<OrderViewDTO>> GetOrderDetails(int userId);

        Task<AdminOrderOutputDTO> GetOrderDetailById(int orderId);

        Task<bool> UpdateOrder(int orderId, AdminOrderOutputDTO adminOrder);

    }
}
