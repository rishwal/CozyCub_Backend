using CozyCub.Models.ProductModels;
using System.ComponentModel.DataAnnotations;

namespace CozyCub.Models.Orders
{
    public class OrderedItem
    {
    public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        public int Quantity {  get; set; }

        
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
