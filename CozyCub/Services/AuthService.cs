using AutoMapper;
using CozyCub.Interfaces;
using CozyCub.Models.User.DTOs;
using CozyCub.Models.UserModels;
using BCrypt.Net;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using CozyCub.Helpers;
using Microsoft.AspNetCore.Identity;

namespace CozyCub.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AuthService(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<string> Register(UserRegisterDTO userDTO)
        {
            if (userDTO.Id < 0)
                return null;

            var isExist = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDTO.Email);

            if (isExist != null)
                return "User exists";

            var userEntity = _mapper.Map<User>(userDTO);

            userEntity.Password = HashPassword(userDTO.Password);

            _context.Users.Add(userEntity);

            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(userEntity);

            return token;
        }

        private ClaimsIdentity GetClaimsIdentity(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name , user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,user.Roles)
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

            var userEntity = await _context.Users.FirstOrDefaultAsync(user => user.UserName == userDTO.UserName);

            if (userEntity == null || !validatePassword(userDTO.Password, userEntity.Password))
            {
                throw new InvalidOperationException("Invalid username or password");
            }

            var token = GenerateJwtToken(userEntity);

            return token;


        }

        private string GenerateJwtToken(User user)
        {
            var claimIdentity = GetClaimsIdentity(user);

            var token = JwtHelper.GenerateJwtToken(_configuration["Jwt:SecretKey"], _configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claimIdentity);

            return token;
        }


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


