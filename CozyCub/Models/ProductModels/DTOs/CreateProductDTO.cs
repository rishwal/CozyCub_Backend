using System.ComponentModel.DataAnnotations;

namespace CozyCub.Models.ProductModels.DTOs
{
    public class CreateProductDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal OfferPrice { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(500, ErrorMessage = "Product description should'nt be greater than 500 characters")]
        public string Description { get; set; }

        [Required]
        public int CategoryId { get; set; }



    }
}
