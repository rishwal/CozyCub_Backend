using CozyCub.Models.UserModels;
using System.ComponentModel.DataAnnotations;

namespace CozyCub.Models.Orders
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string CustomerPhone { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string OrderSatus { get; set; }

        [Required]
        public string OrderString { get; set; }

        [Required]
        public string TransactionId { get; set; }

        public UserModels.User user { get; set; }
        public List<oredrI>
    }
}
