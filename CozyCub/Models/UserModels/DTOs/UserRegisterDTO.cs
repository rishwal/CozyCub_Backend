using System.ComponentModel.DataAnnotations;

namespace CozyCub.Models.UserModels.DTOs
{
    public class UserRegisterDTO
    {

        [Required]
        public string UserName { get; set; }

        [Required]

        [EmailAddress]
        public string Email { get; set;}

        [Required]
        public string Password { get; set;} 

  
    }
}
