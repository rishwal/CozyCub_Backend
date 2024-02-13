using CozyCub.Models.Wishlist.DTOs;

namespace CozyCub.Services.WishList
{
    public interface IWishListService
    {
        Task<bool> AddToWishList(int userId, int productId);
        Task<bool> RemoveWishList(int productId);
        Task<List<WishListOutputDTO>> GetWishList(int userId);
    }
}
