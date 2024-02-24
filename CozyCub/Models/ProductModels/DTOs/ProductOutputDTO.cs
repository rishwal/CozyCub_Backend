namespace CozyCub.Models.ProductModels.DTOs
{
    public class ProductOutputDTO
    {

        public int Id { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public string Category { get; set; }

        public decimal Price { get; set; }
            
        public char Gender { get; set; }

        public decimal OfferPrice { get; set; }

        public string ProductImage { get; set; }

    }
}
