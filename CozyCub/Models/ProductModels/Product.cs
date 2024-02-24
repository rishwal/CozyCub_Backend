using CozyCub.Models.CartModels;
using CozyCub.Models.Classification;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CozyCub.Models.ProductModels
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Product description is required.")]
        public string Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Offer price must be greater than or equal to 0.")]
        public decimal OfferPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Image URL is required.")]
        [Url(ErrorMessage = "Invalid URL format.")]
        public string Image { get; set; }

        // 1=male , 2=female
        public char Gender { get; set; }

        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Category ID is required.")]
        public int CategoryId { get; set; }




        // Navigation property to represent the category associated with this product
        public virtual Category Category { get; set; }

        // Navigation property to represent the cart items associated with this product
        public virtual List<CartItem> CartItems { get; set; }
    }
}
