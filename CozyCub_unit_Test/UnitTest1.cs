//using CozyCub;
//using CozyCub.Models.UserModels.DTOs;
//using CozyCub.Services.Auth;
//using Microsoft.EntityFrameworkCore;

//namespace CozyCub_unit_Test
//{
//    public class AuthserviceTests
//    {
//        [Fact]
//        public async Task Register_ValidaUser_ReturnsSuccessMessage()
//        {
//            var userDTO = new UserRegisterDTO
//            {
//                UserName = "Rishwal",
//                Password = "admin@123",

//            };

//            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//            .UseInMemoryDatabase(databaseName: "cozycub")
//            .Options;

//            using(var context = new ApplicationDbContext(options))
//            {
//                var authServices = new AuthService(context, null, null);

//                var result = await authServices.Register(userDTO);

//                Assert.Equal("userr registered successfully !", result);
//            }

//        }
//    }
//}