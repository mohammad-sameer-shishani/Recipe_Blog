using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipe_Blog.Models;

public partial class User
{
    public decimal Id { get; set; }
	[Display(Name = "First Name")]
	public string? Firstname { get; set; }
	[Display(Name = "Last Name")]
	public string? Lastname { get; set; }
	[Display(Name = "Birth Date")]
	public DateTime? Birthdate { get; set; }

    public decimal? RoleId { get; set; }
	[Display(Name = "Image Path")]
	public string? Imgpath { get; set; }
	[NotMapped]
	[Display(Name = "User Image")]
	public IFormFile? ImageFile { get; set; }
	public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Login> Logins { get; set; } = new List<Login>();

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();

    public virtual ICollection<Visa> Visas { get; set; } = new List<Visa>();
}
