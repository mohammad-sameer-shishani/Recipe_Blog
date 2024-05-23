using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipe_Blog.Models;

public partial class User
{
    public decimal Id { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public DateTime? Birthdate { get; set; }

    public decimal? RoleId { get; set; }

    public string? Imgpath { get; set; }
    [NotMapped]
    public IFormFile? ImageFile { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Login> Logins { get; set; } = new List<Login>();

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();

    public virtual ICollection<Visa> Visas { get; set; } = new List<Visa>();
}
