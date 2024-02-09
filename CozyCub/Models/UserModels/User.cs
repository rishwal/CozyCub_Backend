using CozyCub.Models.CartModels;
using CozyCub.Models.Orders;
using CozyCub.Models.Wishlist;
using System.ComponentModel.DataAnnotations;

namespace CozyCub.Models.UserModels
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(50, ErrorMessage = "Name should not exceed 100 characters !")]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        [MinLength(8)]
        public string Password { get; set; }

        //User Roles
        public string Role { get; set; }

        public bool Banned { get; set; }

        public virtual Cart cart { get; set; }

        public virtual List<Order> Orders { get; set; }
        public virtual List<WishList> WishLists { get; set; }

    }
}
