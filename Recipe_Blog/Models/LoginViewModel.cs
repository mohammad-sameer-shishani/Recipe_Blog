using System.ComponentModel.DataAnnotations;

namespace Recipe_Blog.Models
{
	public class LoginViewModel
	{
		[Required]
		[StringLength(50)]
		public string Password { get; set; } = null!;
		[Display(Name = "User Name")]
		public string UserName { get; set; } = null!;

	}
}
