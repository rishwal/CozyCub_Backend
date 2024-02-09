using System.ComponentModel.DataAnnotations;
using CozyCub.Models.UserModels;

namespace CozyCub.Models.CartModels
{
    public class Cart
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public virtual UserModels.User User { get; set; }

        public virtual List<CartItem> CartItems { get; set; }

    }
}
