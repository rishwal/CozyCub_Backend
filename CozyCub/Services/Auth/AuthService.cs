using AutoMapper;
using CozyCub.Models.UserModels.DTOs;
using CozyCub.Models.UserModels;
using BCrypt.Net;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using CozyCub.Helpers;
using Microsoft.AspNetCore.Identity;

namespace CozyCub.Services.Auth
{
    /// <summary>
    /// Service responsible for user authentication and authorization.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        public AuthService(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        // <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userDTO">User data.</param>
        /// <returns>JWT token on successful registration, or null if user ID is invalid.</returns>
        public async Task<string> Register(UserRegisterDTO userDTO)
        {
            try
            {
                if (userDTO == null)
                    throw new ArgumentNullException(nameof(userDTO), "User data cannot be null.");

                if (userDTO.Id < 0)
                    throw new ArgumentException("Invalid user ID.", nameof(userDTO.Id));

                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDTO.Email);
                if (existingUser != null)
                    throw new InvalidOperationException("User with the same email already exists.");

                var userEntity = _mapper.Map<User>(userDTO);
                userEntity.Password = HashPassword(userDTO.Password);
                _context.Users.Add(userEntity);
                await _context.SaveChangesAsync();

                return "User registered successfully !";
            }
            catch (Exception ex)
            {
                return $"Failed to register user ,Please try again! {ex.Message}";
                throw;
            }
        }

        private ClaimsIdentity GetClaimsIdentity(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name , user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,user.Role)
            };

            //if (user.Roles != null && user.Roles.Any())
            //{
            //    claims.AddRange(user.Roles.Select(role => new Claim(C)));
            //}

            return new ClaimsIdentity(claims, "custom");


        }

        //Function for lohining in.
        public async Task<string> Login(UserLoginDTO userDTO)
        {

            var userEntity = await _context.Users.FirstOrDefaultAsync(user => user.Email == userDTO.Email);

            if (userEntity == null || !validatePassword(userDTO.Password, userEntity.Password))
            {
                throw new InvalidOperationException("Invalid email or password");
            }



            var token = GenerateJwtToken(userEntity);

            return token;


        }

        // Other methods...

        #region Private Methods

        // Add XML comments for private methods explaining their purpose
        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">User for whom the token is generated.</param>
        /// <returns>Generated JWT token.</returns>
        private string GenerateJwtToken(User user)
        {
            var claimIdentity = GetClaimsIdentity(user);
            var token = JwtHelper.GenerateJwtToken(
                _configuration["Jwt:SecretKey"],
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claimIdentity);

            return token;
        }

        // Other private methods...

        #endregion


        //Function to hash the user password.
        public string HashPassword(string password)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        //Function to verify the password when login in.
        private bool validatePassword(string password, string hashPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashPassword);
        }


    }
}


