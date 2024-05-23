namespace Recipe_Blog.Models
{
    public class PaymentViewModel
    {
        public string Fullname { get; set; } = null!;
        public string CardId { get; set; } = null!;
        public byte Cvc { get; set; }
        public DateTime Expiredate { get; set; }

    }
}
