using CozyCub.Models.CartModels.DTOs;

namespace CozyCub.Services.CartServices
{
    public interface ICartServices
    {
        Task<List<OutputCartDTO>> GetCartItems(int userId);

        Task<bool> AddToCart(int userId, int productId);

        Task<bool> DeleteFromCart(int userId, int ProductId);

        Task<bool> IncreaseQuantity(int userId, int ProductId);

        Task<bool> DecreaseQuantity(int userId, int productId);
    }
}
