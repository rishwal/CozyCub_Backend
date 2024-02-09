using CozyCub.Models.CartModels;
using CozyCub.Models.Classification;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CozyCub.Models.ProductModels
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal OfferPrice { get; set; }

        public decimal Price { get; set; }

        public byte[] ImageData { get; set; }

        public int Qty { get; set; }

        public int Rating { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public virtual List<CartItem> CartItems { get; set; }




    }
}
