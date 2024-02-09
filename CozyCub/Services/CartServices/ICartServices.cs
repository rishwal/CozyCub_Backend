using CozyCub.Models.CartModels.DTOs;

namespace CozyCub.Services.CartServices
{
    public interface ICartServices
    {
        Task<List<OutputCartDTO>> GetCartItems(int userId);

        Task AddToCArt(int userId, int productId);

        Task DeleteCart(int userId, int ProductId);

        Task IncreaseQuantity(int userId, int ProductId);

        Task DecreaseQuantity(int userId, int productId); 
    }
}
