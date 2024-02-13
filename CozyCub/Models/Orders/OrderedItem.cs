using CozyCub.Models.ProductModels;
using System.ComponentModel.DataAnnotations;

namespace CozyCub.Models.Orders
{
    public class OrderedItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Order ID is required.")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Product ID is required.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Total price is required.")]
        public decimal TotalPrice { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        public int Quantity { get; set; }



        // Navigation property to represent the order associated with this item
        public virtual Order Order { get; set; }

        // Navigation property to represent the product associated with this item
        public virtual Product Product { get; set; }
    }
}
