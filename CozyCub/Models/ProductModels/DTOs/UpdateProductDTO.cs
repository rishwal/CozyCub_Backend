using System.ComponentModel.DataAnnotations;

namespace CozyCub.Models.ProductModels.DTOs
{
    public class UpdateProductDTO
    {
        public string ProductName { get; set; }

        public string Description { get; set; }


        public decimal OfferPrice { get; set; }

        public decimal Price { get; set; }

        public char Gender { get; set; }

        public int Qty { get; set; }

        public string Image { get; set; }

        [Required]
        public int CategoryId { get; set; }


    }
}
