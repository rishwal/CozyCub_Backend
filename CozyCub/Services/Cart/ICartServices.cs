using CozyCub.Models.CartModels.DTOs;

namespace CozyCub.Services.CartServices
{
    public interface ICartServices
    {
        Task<List<OutputCartDTO>> GetCartItems(string token);

        Task<bool> AddToCart(string token, int productId);

        Task<bool> DeleteFromCart(string token, int ProductId);

        Task<bool> IncreaseQuantity(string token, int ProductId);

        Task<bool> DecreaseQuantity(string token, int productId);
    }
}
