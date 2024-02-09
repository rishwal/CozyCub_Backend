using System.ComponentModel;

namespace CozyCub.Models.CartModels.DTOs
{
    public class OutputCartDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public byte[] Image { get; set; }

        public decimal TotalPrice { get; set; }

        public int Quantity { get; set; }

    }
}
