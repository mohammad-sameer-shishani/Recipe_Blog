using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Recipe_Blog.Models;

public partial class Testimonial
{
    [Key]
    public decimal Id { get; set; }

    public DateTimeOffset? Creationdate { get; set; }

    public string? Content { get; set; }

    public decimal? UserId { get; set; }

    public virtual User? User { get; set; }
}
