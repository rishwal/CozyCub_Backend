using CozyCub.Models.CartModels;
using CozyCub.Models.CartModels.DTOs;
using Microsoft.Identity.Client;
using System.Transactions;

namespace CozyCub.Models.Orders.DTOs
{
    public class AdminOrderOutputDTO
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public string CustomerEmail { get; set; }

        public string CustomerCity { get; set; }

        public string CustomerPhone { get; set; }

        public string Address { get; set; }

        public string Orderstring { get; set; }

        public string OrderStatus { get; set; }

        public string TransactionId { get; set; }

        public DateTime OrderDate { get; set; }

        public List<OutputCartDTO> ProductsPurchased { get; set; }
    }
}
