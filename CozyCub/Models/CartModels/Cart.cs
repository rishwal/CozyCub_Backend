using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CozyCub.Models.UserModels;

namespace CozyCub.Models.CartModels
{
    public class Cart
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public int UserId { get; set; }

        // Navigation property to represent the user associated with this cart
        public virtual User User { get; set; }

        // Collection navigation property to represent the items in this cart
        // Add [Required] annotation if CartItems must always be initialized
        public virtual List<CartItem> CartItems { get; set; }
    }
}
