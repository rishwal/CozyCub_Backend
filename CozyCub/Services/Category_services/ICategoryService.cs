using CozyCub.Models.Categories.DTOs;
using CozyCub.Models.Classification;

namespace CozyCub.Services.Category_services
{
    public interface ICategoryService
    {
        public Task<bool> CreateCategory(CategoryCreateDTO category);

        public Task<List<CategoryDTO>> GetAllCategories();

        public Task<bool> UpdateCategory(int id,CategoryCreateDTO category);

        public Task<CategoryDTO> GetCategoryById(int id);

        public Task<CategoryDTO> DeleteCategory(int id);

    }
}
