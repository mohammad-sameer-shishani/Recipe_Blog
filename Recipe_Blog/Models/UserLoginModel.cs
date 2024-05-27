using System.ComponentModel.DataAnnotations;

namespace Recipe_Blog.Models
{
    public class UserLoginModel
    {
        public decimal Id { get; set; }
        [Display(Name = "First Name")]
        public string? Firstname { get; set; }
        [Display(Name = "Last Name")]
        public string? Lastname { get; set; }
        [DataType(DataType.Date)]
		[Display(Name = "Birth Date")]
		public DateTime? Birthdate { get; set; }

        public string? Gender { get; set; }

        public decimal? RoleId { get; set; }
        public string Email { get; set; } = null!;
        [StringLength(50)]
		[Display(Name = "User Name")]
		public string UserName { get; set; } = null!;
    }
}
