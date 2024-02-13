using System.ComponentModel.DataAnnotations;

namespace CozyCub.Models.Wishlist.DTOs
{
    public class WishListDTO
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int UserId { get; set; }

    }
}
