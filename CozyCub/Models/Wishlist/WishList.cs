using CozyCub.Models.ProductModels;
using CozyCub.Models.UserModels;
using System.ComponentModel.DataAnnotations;

namespace CozyCub.Models.Wishlist
{
    public class WishList
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Product ID is required.")]
        public int ProductId { get; set; }

        public virtual User User { get; set; }

        public virtual Product Product { get; set; }
    }
}
