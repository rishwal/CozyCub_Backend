using CozyCub.Models.UserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CozyCub.Models.Orders
{
    public class Order
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Order date is required.")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Customer name is required.")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Customer email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage = "Customer phone is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string CustomerPhone { get; set; }

        [Required(ErrorMessage = "Customer city is required.")]
        public string CustomerCity { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Order status is required.")]
        public string OrderStatus { get; set; }

        [Required(ErrorMessage = "Order string is required.")]
        public string OrderString { get; set; }

        [Required(ErrorMessage = "Transaction ID is required.")]
        public string TransactionId { get; set; }

        public string OrderId { get; set; }


        // Navigation property to represent the user who placed the order
        public virtual User user { get; set; }

        // Navigation property to represent the items in the order
        public virtual List<OrderedItem> OrderItems { get; set; }
    }
}
