using CozyCub.Models.ProductModels;

namespace CozyCub.Models.Categories
{
    public class CategoryCreateDTO
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

    }
}
