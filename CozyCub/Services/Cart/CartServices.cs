using CozyCub.JWT_Id;
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
        private readonly string _hostUrl;
        private readonly IConfiguration _configuration;
        private readonly IJwtService _jwtservice;

        public CartServices(IConfiguration configuration, ApplicationDbContext context, IJwtService jwtservice)
        {
            _context = context;
            _configuration = configuration;
            _hostUrl = _configuration["HostUrl:url"];
            _jwtservice = jwtservice;

        }

        /// <summary>
        /// Retrieves all product to the user's shopping cart.
        /// </summary>
        public async Task<List<OutputCartDTO>> GetCartItems(string token)
        {
            try
            {
                int userId = _jwtservice.GetUserIdFromToken(token);


                if (userId == 0) throw new Exception("User with id doesn't exist !");

                var user = await _context.Cart
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.product)
                    .FirstOrDefaultAsync(p => p.UserId == userId);
                if (user == null)
                {
                    return [];
                }

                if (user != null)
                {
                    var cartItems = user.CartItems.Select(ci => new OutputCartDTO
                    {
                        Id = ci.ProductId,
                        ProductName = ci.product.Name,
                        Quantity = ci.Quantity,
                        Price = ci.product.Price,
                        OfferPrice = ci.product.OfferPrice,
                        TotalPrice = ci.product.Price * ci.Quantity,
                        Image = _hostUrl + ci.product.Image

                    }).ToList();

                    return cartItems;
                }
                return new List<OutputCartDTO>();

            }

            catch (Exception ex)
            {
                throw new Exception("Something went wring while fetching cart items : 👉🏼 " + ex.Message);
            }
        }


        /// <summary>
        /// Adds a product to the user's shopping cart.
        /// </summary>
        public async Task<bool> AddToCart(string token, int productId)
        {
            try
            {
                int userId = _jwtservice.GetUserIdFromToken(token);

                if (userId == 0) throw new Exception($"User not valid witrh token: {token}");

                var user = await _context.Users
                    .Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                var Product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

                if (Product == null) throw new Exception($"Product with id {productId} not found");

                if (user != null && Product != null)
                {
                    //If user doesnt have a cart (empty cart)
                    if (user.Cart == null)
                    {
                        user.Cart = new Cart
                        {
                            UserId = userId,
                            CartItems = new List<CartItem>()
                        };

                        _context.Cart.Add(user.Cart);
                        await _context.SaveChangesAsync();

                    }


                }

                CartItem? existingCartProduct = user.Cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

                if (existingCartProduct != null)
                {
                    existingCartProduct.Quantity++;
                }
                else
                {
                    CartItem cartItem = new CartItem
                    {
                        CartId = user.Cart.Id,
                        ProductId = productId,
                        Quantity = 1,
                    };

                    _context.CartItems.Add(cartItem);

                }

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error adding product to cart: {ex}");
                return false;

            }
        }


        /// <summary>
        /// Removes a product from the user's shopping cart.
        /// </summary>
        public async Task<bool> DeleteFromCart(string token, int ProductId)
        {
            try
            {
                int userId = _jwtservice.GetUserIdFromToken(token);

                if (userId == 0) throw new Exception("User id is not valid !");

                var user = await _context.Users
                    .Include(u => u.Cart)
                    .ThenInclude(u => u.CartItems)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == ProductId);

                if (user != null && product != null)
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
                throw new Exception($"No User or Product presnt with given id , ProductId:{ProductId} !");
               
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception("An exception occured while deleting a product from the users cart " + ex.Message);
             
            }
        }


        /// <summary>
        /// Increments the quantity of a product in user's shopping cart.
        /// </summary>
        public async Task<bool> IncreaseQuantity(string token, int ProductId)
        {
            try
            {
                int userId = _jwtservice.GetUserIdFromToken(token);

                if (userId == 0)
                    throw new Exception("A user with the current token is not found !");

                var user = await _context.Users
                    .Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                    .FirstOrDefaultAsync(u => u.Id == userId);


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
                throw new Exception("An exception occured while increasing the quantity of the product" + ex.Message);

            }
        }

        /// <summary>
        /// Decrements the quantity of a product in user's shopping cart.
        /// </summary>
        public async Task<bool> DecreaseQuantity(string token, int productId)
        {
            try
            {
                int userId = _jwtservice.GetUserIdFromToken(token);

                if (userId == 0) throw new Exception($"User is not valid with token {token}");


                var user = await _context.Users
                    .Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                    .FirstOrDefaultAsync(u => u.Id == userId);
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

                if (user != null && product != null)
                {
                    var item = user.Cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

                    if (item != null)
                    {
                        item.Quantity = item.Quantity >= 1 ? item.Quantity - 1 : item.Quantity;

                        if (item.Quantity == 0)
                        {
                            _context.CartItems.Remove(item);
                            await _context.SaveChangesAsync();
                        }

                        await _context.SaveChangesAsync();
                    }

                }

                if (user != null)


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
