using System.ComponentModel.DataAnnotations;

namespace Recipe_Blog.Models
{
	public class LoginViewModel
	{
		[Required]
		[StringLength(50)]
		public string Password { get; set; } = null!;
		
        public string UserName { get; set; } = null!;

	}
}
