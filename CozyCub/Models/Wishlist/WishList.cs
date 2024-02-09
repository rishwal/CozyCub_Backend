using CozyCub.Models.ProductModels;
using CozyCub.Models.UserModels;
using System.ComponentModel.DataAnnotations;

namespace CozyCub.Models.Wishlist
{
    public class WishList
    {
        public int Id { get; set; }

        [Required]
        public int UserId {  get; set; }

        [Required]
        public int ProductId { get; set; }

        public virtual UserModels.User User { get; set; }
        public virtual Product Products { get; set; }

    }
}
