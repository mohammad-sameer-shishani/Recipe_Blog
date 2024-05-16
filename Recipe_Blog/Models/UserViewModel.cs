using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Recipe_Blog.Models
{
    public class UserViewModel
    {
        [Key]
		[Display(Name = "First Name")]
		public string? Firstname { get; set; }
		[Display(Name = "Last Name")]
		public string? Lastname { get; set; }
        //[DataType(DataType.Date)]
        public DateTime? Birthdate { get; set; }
        [Required]
		[StringLength(100)]
		public string Email { get; set; } = null!;
        [Required]
		[StringLength(50)]
		public string Password { get; set; } = null!;
        [Required]
		[StringLength(50)]
		public string UserName { get; set; } = null!;
        [NotMapped]
        public IFormFile? imageFile { get; set; }
        public decimal? UserId { get; set; }
		[NotMapped]
		public decimal? GenderId { get; set; }
		public decimal Roleid { get; set; } 
		public string? Gender { get; set; }
	}
}
