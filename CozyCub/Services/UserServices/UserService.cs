using AutoMapper;
using CozyCub.Models.UserModels.DTOs;
using CozyCub.Models.UserModels.DTOs;
using CozyCub.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace CozyCub.Services.UserServices
{
    /// <summary>
    /// Service for user-related operations.
    /// </summary>
    public class UserService : IUserService
    {

        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
    


        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        public UserService(ApplicationDbContext context, IMapper mappper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mappper;

        }


        /// <summary>
        /// Bans a user by setting their <see cref="User.Banned"/> property to true.
        /// </summary>
        /// <param name="userId">The ID of the user to ban.</param>
        /// <returns>True if the user was successfully banned; otherwise, false.</returns>
        public async Task<bool> BanUser(int userId)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    // User with the given ID not found
                    return false;
                }

                // Ban the user
                user.Banned = true;
                await _context.SaveChangesAsync();
                return true; // User banned successfully
            }
            catch (Exception ex)
            {
                // Log and handle the exception
                Console.WriteLine($"Error banning user with ID {userId}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>The user with the specified ID.</returns>
        public async Task<OutPutUser> GetUserById(int id)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    // User with the given ID not found
                    return null;
                }

                // Map the user entity to the output DTO
                return _mapper.Map<OutPutUser>(user);
            }
            catch (Exception ex)
            {
                // Log and handle the exception
                Console.WriteLine($"Error retrieving user with ID {id}: {ex.Message}");
                return null;
            }
        }




        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>A list of all users.</returns>
        public async Task<List<OutPutUser>> GetUsers()
        {
            try
            {
                // Retrieve all users from the database
                var users = await _context.Users.ToListAsync();

                // Map the list of user entities to a list of output DTOs
                var mappedUsers = _mapper.Map<List<OutPutUser>>(users);

                // Return the list of mapped users
                return mappedUsers;
            }
            catch (Exception ex)
            {
                // Log and handle the exception
                Console.WriteLine($"Error retrieving users: {ex.Message}");
                return null; // Or throw an exception based on your error handling strategy
            }
        }



        /// <summary>
        /// Unbans a user by setting their <see cref="User.Banned"/> property to false.
        /// </summary>
        /// <param name="userId">The ID of the user to unban.</param>
        /// <returns>True if the user was successfully unbanned; otherwise, false.</returns>
        public async Task<bool> UnBanUser(int userId)
        {
            try
            {
                // Find the user in the database by ID
                var user = await _context.Users.FindAsync(userId);

                // Check if the user exists
                if (user == null)
                {
                    return false; // User not found
                }

                // Update the 'Banned' property to false
                user.Banned = false;

                // Save changes to the database
                await _context.SaveChangesAsync();

                return true; // User unbanned successfully
            }
            catch (Exception ex)
            {
                // Log and handle the exception
                Console.WriteLine($"Error unbanning user: {ex.Message}");
                return false; // Or throw an exception based on your error handling strategy
            }
        }

    }
}
