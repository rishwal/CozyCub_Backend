using AutoMapper;
using CozyCub.Models.Wishlist.DTOs;
using Microsoft.EntityFrameworkCore;
using CozyCub.Models.Wishlist;
using CozyCub.Services.WishList;
using Razorpay.Api;


namespace CozyCub.Services.WishList_Services
{
    /// <summary>
    /// Service for managing wishlists.
    /// </summary>
    public class WishListService : IWishListService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        /// <summary>
        /// Initializes a new instance of the <see cref="WishListService"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        public WishListService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        /// <summary>
        /// Adds a product to the user's wishlist.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="productId">The ID of the product to add to the wishlist.</param>
        /// <returns>True if the product was successfully added to the wishlist; otherwise, false.</returns>

        public async Task<bool> AddToWishList(int userId, int productId)
        {
            try
            {
                // Check if the item already exists in the wishlist
                var itemExists = await _context.WishLists
                    .AnyAsync(w => w.UserId == userId && w.ProductId == productId);

                if (!itemExists)
                {
                    // Create a new wishlist item DTO
                    var wishListDTO = new WishListDTO
                    {
                        UserId = userId,
                        ProductId = productId
                    };

                    var wishList = _mapper.Map<CozyCub.Models.Wishlist.WishList>(wishListDTO);

                    // Add the new wishlist item to the database
                    _context.WishLists.Add(wishList);

                    // Save changes to the database
                    await _context.SaveChangesAsync();

                    return true;
                }
                else
                {
                    return false;

                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding product to wishlist: {ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// Retrieves the user's wishlist.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The user's wishlist.</returns>
        public async Task<List<WishListOutputDTO>> GetWishList(int userId)
        {
            try
            {
                var wishList = await _context.WishLists
                    .Include(w => w.Product)
                    .ThenInclude(p => p.Category)
                    .Where(u => u.UserId == userId)
                    .ToListAsync();

                if (wishList.Count > 0)
                {
                    return wishList.Select(w => new WishListOutputDTO
                    {
                        Id = w.Product.Id,
                        ProductName = w.Product.Name,
                        ProductDescription = w.Product.Description,
                        Price = w.Product.Price,
                        Category = w.Product.Category?.Name,
                        ProductImage = w.Product.Image,

                    }).ToList();
                }
                else
                {
                    return new List<WishListOutputDTO>();

                }

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return new List<WishListOutputDTO>();
            }
        }


        /// <summary>
        /// Removes a product from the user's wishlist.
        /// </summary>
        /// <param name="productId">The ID of the product to remove from the wishlist.</param>
        /// <returns>True if the product was successfully removed from the wishlist; otherwise, false.</returns>
        public async Task<bool> RemoveWishList(int productId)
        {
            try
            {
                // Find the wishlist item by product ID
                var wishList = await _context.WishLists.FirstOrDefaultAsync(p => p.ProductId == productId);

                if (wishList != null)
                {

                    // Remove the wishlist item from the database
                    _context.WishLists.Remove(wishList);


                    // Save changes to the database
                    await _context.SaveChangesAsync();

                    return true; //Removed suceesfuly 
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing product from wishlist: {ex.Message}");
                return false;


            }
        }
    }
}
