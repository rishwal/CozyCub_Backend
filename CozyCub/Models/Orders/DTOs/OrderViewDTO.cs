using CozyCub.Models;
using CozyCub.Models.UserModels.DTOs;
using CozyCub.Models.UserModels;

namespace CozyCub.Models.Orders.DTOs
{
    public class OrderViewDTO
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime OrderDate { get; set; }

        public string Image { get; set; }

        public string OrderId { get; set; }

        public string OrderStatus { get; set; }

 

    }
}
