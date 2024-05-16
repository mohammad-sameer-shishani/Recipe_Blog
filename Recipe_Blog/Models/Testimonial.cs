using System;
using System.Collections.Generic;

namespace Recipe_Blog.Models;

public partial class Testimonial
{
    public decimal Id { get; set; }

    public DateTimeOffset? Creationdate { get; set; }

    public string? Content { get; set; }

    public decimal? UserId { get; set; }

    public decimal? TestimonialStatusId { get; set; }

    public virtual Status? TestimonialStatus { get; set; }

    public virtual User? User { get; set; }
}
