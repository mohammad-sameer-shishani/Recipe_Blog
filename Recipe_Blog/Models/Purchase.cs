namespace Recipe_Blog.Models
{
	public class Purchase
	{
		public decimal RecipeId { get; set; }	

		public decimal VisaId { get; set; }

		public long Cardnumber { get; set; }

		public byte Cvc { get; set; }

		public string Nameoncard { get; set; } = null!;

        public string? Expdate { get; set; }

		public decimal? Amount { get; set; }

		public decimal? UserId { get; set; }



	}
}
