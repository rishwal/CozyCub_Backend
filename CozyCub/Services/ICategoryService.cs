using CozyCub.Models.Classification;

namespace CozyCub.Services
{
    public interface ICategoryService
    {
        public Task<List<Category>> categories();
    }
}
