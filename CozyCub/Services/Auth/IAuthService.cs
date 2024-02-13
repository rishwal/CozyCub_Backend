using CozyCub.Models.UserModels.DTOs;

namespace CozyCub.Services.Auth
{
    public interface IAuthService
    {
        Task<string> Register(UserRegisterDTO userDTO);
        Task<string> Login(UserLoginDTO userDTO);
    }
}
