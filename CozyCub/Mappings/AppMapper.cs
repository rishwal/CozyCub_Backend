using AutoMapper;
using CozyCub.Models.Categories.DTOs;
using CozyCub.Models.Classification;
using CozyCub.Models.ProductModels;
using CozyCub.Models.ProductModels.DTOs;
using CozyCub.Models.UserModels.DTOs;
using CozyCub.Models.UserModels;
using CozyCub.Models.Wishlist;
using CozyCub.Models.Wishlist.DTOs;

namespace CozyCub.Mappings
{
    /// <summary>
    /// Represents AutoMapper configuration for mapping between domain models and DTOs.
    /// </summary>
    public class AppMapper : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppMapper"/> class.
        /// Configures mappings between domain models and DTOs.
        /// </summary>
        public AppMapper()
        {
            // User to UserRegisterDTO and vice versa mapping
            CreateMap<User, UserRegisterDTO>().ReverseMap();

            // Product to ProductDTO and vice versa mapping
            CreateMap<Product, UpdateProduct>().ReverseMap();

            CreateMap<Product , CreateProductDTO>().ReverseMap();   

            // Category to CategoryDTO and vice versa mapping
            CreateMap<Category, CategoryDTO>().ReverseMap();

            // UserDTO to User and vice versa mapping
            CreateMap<OutPutUser, User>().ReverseMap();

            // Category to CategoryCreateDTO and vice versa mapping
            CreateMap<Category, CategoryCreateDTO>().ReverseMap();

            // WishList to WishListDTO and vice versa mapping
            CreateMap<WishList, WishListDTO>().ReverseMap();
        }
    }
}
