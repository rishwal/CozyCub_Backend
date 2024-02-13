using System.ComponentModel.DataAnnotations;

namespace CozyCub.Models.ProductModels.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal OfferPrice { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string ImageData { get; set; }

        [Required]
        public int CategoryId { get; set; }

    }
}
