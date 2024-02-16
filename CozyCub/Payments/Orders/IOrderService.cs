using CozyCub.Models.Orders.DTOs;
using CozyCub.Payments;

namespace CozyCub.Payments.Orders
{
    public interface IOrderService
    {

        Task<bool> CreateOrder(string token, OrderRequestDTO requestDTO);

        Task<decimal> GetTotalRevenue();

        Task<List<OrderViewDTO>> GetOrderDetails(string token);

        Task<List<AdminOrderOutputDTO>> GetOrderDetailsForAdmin();

        Task<AdminOrderOutputDTO> GetOrderDetailById(int orderIdF);

        Task<bool> UpdateOrder(int orderId, AdminOrderOutputDTO adminOrder);

        //razor pay method 
        Task<string> RazorPayPayment(long price);
        public List<OrderDetailDTO> payment(RazorPayDTO razorPayDTO);
    }
}
