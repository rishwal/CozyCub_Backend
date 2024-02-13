using System.ComponentModel.DataAnnotations;

namespace CozyCub.Models.UserModels.DTOs
{
    public class UserLoginDTO
    {
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }

        [Required] 
        public string Password { get; set; }

    }
}
