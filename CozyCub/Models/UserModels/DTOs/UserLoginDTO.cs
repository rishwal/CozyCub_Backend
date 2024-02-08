using System.ComponentModel.DataAnnotations;

namespace CozyCub.Models.User.DTOs
{
    public class UserLoginDTO
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required] 
        public string Password { get; set; }
    }
}
