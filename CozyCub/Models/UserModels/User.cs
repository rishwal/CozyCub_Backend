using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CozyCub.Models.CartModels;
using CozyCub.Models.Orders;
using CozyCub.Models.Wishlist;

namespace CozyCub.Models.UserModels
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "User name is required.")]
        [StringLength(50, ErrorMessage = "Name should not exceed 50 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; }

        //User Roles
        public string Role { get; set; }
        public bool Banned { get; set; }

        // Navigation properties
        public virtual Cart Cart { get; set; }
        public virtual List<Order> Orders { get; set; }
        public virtual List<WishList> WishLists { get; set; }
    }
}
