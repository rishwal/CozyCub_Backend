namespace CozyCub.Models.Wishlist.DTOs
{
    public class WishListOutputDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }

        public string ProductDescription { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }

        public string ProductImage { get; set; }
    }
}
