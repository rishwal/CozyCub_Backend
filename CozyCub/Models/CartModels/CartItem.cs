using CozyCub.Models.ProductModels;
using System.ComponentModel.DataAnnotations;

namespace CozyCub.Models.CartModels
{
    public class CartItem
    {
        public int Id { get; set; }

        [Required]
        public int CartId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        public virtual Cart cart { get; set; }
        public virtual Product product { get; set; }
          

        
    }
}
