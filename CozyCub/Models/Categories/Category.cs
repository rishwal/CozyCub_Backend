using CozyCub.Models.ProductModels;

namespace CozyCub.Models.Classification
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Product> Products { get; set; }
        

    }
}
