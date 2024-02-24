using System.ComponentModel.DataAnnotations;

namespace CozyCub.Models.ProductModels.DTOs
{
    public class UpdateProduct
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal OfferPrice { get; set; }

        [Required]
        public decimal Price { get; set; }

        public char Gender { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        public int CategoryId { get; set; }

    }
}
