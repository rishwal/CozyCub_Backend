using System.ComponentModel.DataAnnotations.Schema;

namespace CozyCub.Models.Orders.DTOs
{
    public class OrderDetailDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string Mobile { get; set; }

        public double TotalPrice { get; set; }

        [NotMapped]
        public string TransactionId { get; set; }

        [NotMapped]
        public string OrderId { get; set; }
    }
}
