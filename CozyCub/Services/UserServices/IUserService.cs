using CozyCub.Models.UserModels.DTOs;
using CozyCub.Models.UserModels.DTOs;
using CozyCub.Models.UserModels;

namespace CozyCub.Services.UserServices
{
    public interface IUserService
    {
        Task<List<OutPutUser>> GetUsers();
        Task<OutPutUser> GetUserById(int id);
        Task<bool> BanUser(int userId);
        Task<bool> UnBanUser(int userId);
    }
}