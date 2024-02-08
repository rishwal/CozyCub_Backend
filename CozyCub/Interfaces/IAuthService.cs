using CozyCub.Models.User.DTOs;

namespace CozyCub.Interfaces
{
    public interface IAuthService
    {
        Task<string> Register(UserRegisterDTO userDTO);
        Task<string> Login(UserLoginDTO userDTO);
    }
}
