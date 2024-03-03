using CozyCub.Models.ProductModels.DTOs;

namespace CozyCub.Services.ProductService
{
    public interface IProductService
    {
        Task<List<ProductOutputDTO>> GetProducts();
        Task<ProductOutputDTO> GetProductById(int id);
        Task<List<ProductOutputDTO>> GetProductByCategory(int categoryId);

        Task<List<ProductOutputDTO>> GetProductByCategoryName(string category);

        Task<List<ProductOutputDTO>> ProductPagination(int pageNumber = 1, int pageSize = 10);

        Task<List<ProductOutputDTO>> GetClothesByGender(char gender);

        Task<bool> CreateProduct(CreateProductDTO productDTO, IFormFile image);

        Task<bool> UpdateProduct(int id,CreateProductDTO productDTO, IFormFile image);

        Task<bool> DeleteProduct(int productId);
    }
}
