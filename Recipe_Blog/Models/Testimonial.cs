using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Recipe_Blog.Models;

public partial class Testimonial
{
    public decimal Id { get; set; }
	[Display(Name = "Created At")]
	public DateTimeOffset? Creationdate { get; set; }

    public string? Content { get; set; }

    public decimal? UserId { get; set; }
	[Display(Name = "Status")]
	public decimal? TestimonialStatusId { get; set; }
	[Display(Name = "Status")]
	public virtual Status? TestimonialStatus { get; set; }

    public virtual User? User { get; set; }
}
