using System.ComponentModel.DataAnnotations;

namespace CozyCub.Models.UserModels.DTOs
{
    public class UserLoginDTO
    {
        [Required]
        public string Email { get; set; }

        [Required] 
        public string Password { get; set; }

    }
}
