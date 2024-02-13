using System.ComponentModel.DataAnnotations;

namespace CozyCub.Payments
{
    public class RazorPayDTO
    {
        [Required(ErrorMessage = "RazorPay ID is required.")]
        public string razrPayId { get; set; }

        [Required(ErrorMessage = "RazorPay Order ID is required.")]
        public string razrOrdId { get; set; }

        [Required(ErrorMessage = "RazorPay Signature is required.")]
        public string razpaySig { get; set; }
    }
}
