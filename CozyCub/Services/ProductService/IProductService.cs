using CozyCub.Models.ProductModels.DTOs;

namespace CozyCub.Services.ProductService
{
    public interface IProductService
    {
        Task<List<ProductOutputDTO>> GetProducts();
        Task<ProductOutputDTO> GetProductById(int id);
        Task<List<ProductOutputDTO>> GetProductByCategory(int categoryId);

        Task<List<ProductOutputDTO>> ProductPagination(int pageNumber = 1, int pageSize = 10);

        Task AddProduct(CreateProductDTO productDTO, IFormFile image);

        Task UpdateProduct(int id, CreateProductDTO productDTO, IFormFile image);

        Task DeleteProduct(int id);
    }
}
