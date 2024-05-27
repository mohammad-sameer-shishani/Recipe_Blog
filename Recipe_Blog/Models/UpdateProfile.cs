using System.ComponentModel.DataAnnotations;

namespace Recipe_Blog.Models
{
    public class UpdateProfile
    {
        public decimal Id { get; set; }
        [Display(Name = "First Name")]
        public string? Firstname { get; set; }
        [Display(Name = "Last Name")]
        public string? Lastname { get; set; }
		[Display(Name = "Birth Date")]
		public DateTime? Birthdate { get; set; }

        public string? Gender { get; set; }
        public string Email { get; set; } = null!;
        [StringLength(50)]
		[Display(Name = "User Name")]
		public string UserName { get; set; } = null!;
        [Required]
        [StringLength(50)]
        public string Password { get; set; } = null!;
    }
}
