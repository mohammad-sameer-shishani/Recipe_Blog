using System;
using System.Collections.Generic;

namespace Recipe_Blog.Models;

public partial class Status
{
    public decimal Statusid { get; set; }

    public string Statusname { get; set; } = null!;

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();
}
