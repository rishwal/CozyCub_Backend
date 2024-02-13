namespace CozyCub.Models.UserModels.DTOs
{
    public class UserDTO
    {
        //UserDTO removed sensitive properties like password 
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public string Role { get; set; }
    }
}
