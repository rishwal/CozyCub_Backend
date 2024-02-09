﻿namespace CozyCub.Models.CartModels
{
    public class OutPutCart
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public byte[] Image { get; set; }

        public string TotalPrice { get; set; }

        public int Quantity { get; set; }


    }
}
