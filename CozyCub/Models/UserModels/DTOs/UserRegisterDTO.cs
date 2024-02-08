using System.ComponentModel.DataAnnotations;

namespace CozyCub.Models.User.DTOs
{
    public class UserRegisterDTO
    {
        public int Id { get; set; } 
        [Required]
        public string UserName { get; set; }

        [Required]

        [EmailAddress]
        public string Email { get; set;}

        [Required]
        public string Password { get; set;} 
    }
}
