using CozyCub.Models.CartModels;
using CozyCub.Models.CartModels.DTOs;
using CozyCub.Models.ProductModels;
using CozyCub.Models.UserModels;
using CozyCub.Models.UserModels.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace CozyCub.Services.CartServices
{
    public class CartServices : ICartServices
    {
        private readonly ApplicationDbContext _context;

        public CartServices(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        #region Private Methods

        /// <summary>
        /// Retrieves the user with associated cart and cart items.
        /// </summary>
        private async Task<User> GetUserWithCart(int userId)
        {
            return await _context.Users
                .Include(u => u.Cart)
                .ThenInclude(c => c.CartItems)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        #endregion

        /// <summary>
        /// Adds a product to the user's shopping cart.
        /// </summary>
        public async Task<bool> AddToCart(int userId, int productId)
        {
            try
            {
                var user = await GetUserWithCart(userId);
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

                if (user != null && product != null)
                {
                    // Check if the user has a cart, create one if not
                    if (user.Cart == null)
                    {
                        user.Cart = new Cart { UserId = userId, CartItems = new List<CartItem>() };
                        await _context.Cart.AddAsync(user.Cart);
                        await _context.SaveChangesAsync();
                    }

                    var existingItem = user.Cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

                    if (existingItem != null)
                    {
                        existingItem.Quantity++;
                    }
                    else
                    {
                        var newCartItem = new CartItem { CartId = user.Cart.Id, ProductId = productId, Quantity = 1 };
                        await _context.CartItems.AddAsync(newCartItem);
                    }

                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error adding product to cart: {ex.Message}");
                throw;
            }
        }


        /// <summary>
        /// Removes a product from the user's shopping cart.
        /// </summary>
        public async Task<bool> DeleteFromCart(int userId, int ProductId)
        {
            try
            {
                var user = await GetUserWithCart(userId);

                var Product = await _context.Products.FirstOrDefaultAsync(p => p.Id == ProductId);

                if (user != null && Product != null)
                {
                    var item = user.Cart.CartItems.FirstOrDefault(ci => ci.ProductId == ProductId);

                    if (item != null)
                    {
                        _context.CartItems.Remove(item);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Retrieves all product to the user's shopping cart.
        /// </summary>
        public async Task<List<OutputCartDTO>> GetCartItems(int userId)
        {
            try
            {
                var user = await GetUserWithCart(userId);

                if (user != null)
                {
                    var cartItems = user.Cart.CartItems
                        .Where(ci => ci.product != null)
                        .Select(ci => new OutputCartDTO()
                        {
                            Id = ci.ProductId,
                            ProductName = ci.product.Name,
                            Quantity = ci.product.Qty,
                            Price = ci.product.Price,
                            TotalPrice = ci.product.OfferPrice * ci.Quantity,
                            Image = ci.product.Image

                        }).ToList();

                    return cartItems;
                }
                else
                    return new List<OutputCartDTO>();

            }

            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// Increments the quantity of a product in user's shopping cart.
        /// </summary>
        public async Task<bool> IncreaseQuantity(int userId, int ProductId)
        {
            try
            {
                var user = await GetUserWithCart(userId);

                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == ProductId);

                if (user != null && product != null)
                {
                    var item = user.Cart.CartItems.FirstOrDefault(ci => ci.ProductId == ProductId);
                    if (item != null)
                    {
                        item.Quantity++;
                        await _context.SaveChangesAsync();


                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Decrements the quantity of a product in user's shopping cart.
        /// </summary>
        public async Task<bool> DecreaseQuantity(int userId, int productId)
        {
            try
            {
                var user = await GetUserWithCart(userId);

                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

                if (user != null && product != null)
                {
                    var item = user.Cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
                    if (item != null)
                    {
                        item.Quantity--;
                        await _context.SaveChangesAsync();


                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                throw;
            }
        }
    }
}
