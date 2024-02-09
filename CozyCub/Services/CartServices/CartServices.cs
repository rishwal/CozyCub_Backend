using CozyCub.Models.CartModels.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CozyCub.Services.CartServices
{
    public class CartServices : ICartServices
    {
        private readonly ApplicationDbContext _context;

        public CartServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddToCArt(int userId, int productId)
        {

        }

        public Task DecreaseQuantity(int userId, int productId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCart(int userId, int ProductId)
        {
            throw new NotImplementedException();
        }

        public Task<List<OutputCartDTO>> GetCartItems(int userId)
        {
        }

        public Task IncreaseQuantity(int userId, int ProductId)
        {
            throw new NotImplementedException();
        }
    }
}
