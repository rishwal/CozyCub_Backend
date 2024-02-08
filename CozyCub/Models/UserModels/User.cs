using System.ComponentModel.DataAnnotations;

namespace CozyCub.Models.UserModels
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(100, ErrorMessage = "Name should not exceed 100 characters !")]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        //User Roles
        public string Roles { get; set; }

    }
}
