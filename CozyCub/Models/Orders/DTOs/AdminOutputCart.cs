using CozyCub.Models.CartModels;
using System.Transactions;

namespace CozyCub.Models.Orders.DTOs
{
    public class AdminOutputCart
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public string CustomerEmail { get; set; }

        public long CustomerPhone { get; set; }

        public string Address { get; set; }

        public string Orderstring { get; set; }

        public string OrderStatus { get; set; }

        public string TransacionId { get; set; }

        public DateTime OrderDate { get; set; }

        public List<OutPutCart> ProductsPurchased { get; set; }
    }
}
