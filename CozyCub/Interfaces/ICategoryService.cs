using CozyCub.Models.Classification;

namespace CozyCub.Interfaces
{
    public interface ICategoryService
    {
        public Task<List<Catef>> GetCategories();
        public Task<categoryDTO>


    }
}
