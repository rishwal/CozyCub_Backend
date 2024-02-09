using CozyCub.Models.ProductModels.DTOs;

namespace CozyCub.Models.Categories.DTOs
{
    public class CategoryDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal OfferPrice { get; set; }
        public List<ProductDTO> Products { get; set; }

        public int CategoryId { get; set; }



    }
}
