using CozyCub.Models.CartModels;

namespace CozyCub.Models.Orders.DTOs
{
    public class OrderRequestDTO
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerCity { get; set; }
        public string Address { get; set; }
        public string HomeAddress { get; set; }
        public string OrderString { get; set; }

        public string TransactionId { get; set; }
        public List<OutPutCart>
    }
}
