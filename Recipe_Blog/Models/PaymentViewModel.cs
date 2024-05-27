using System.ComponentModel.DataAnnotations;

namespace Recipe_Blog.Models
{
    public class PaymentViewModel
    {
		[Display(Name = "Name On Card")]
		public string Fullname { get; set; } = null!;
		[Display(Name = "Card Number")]
		public string CardId { get; set; } = null!;
        public byte Cvc { get; set; }
		[Display(Name = "Exp Date")]
		public DateTime Expiredate { get; set; }

    }
}
