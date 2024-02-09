using CozyCub.Models.Classification;

namespace CozyCub.Services
{
    public interface ICategoryService
    {
        public Task<List<Category>> CreateCategory(Category category);
    
        public Task<List<Category>> GetAllCategories();

        public Task<List<Category>> GetCategorieById(int id);





    }
}
