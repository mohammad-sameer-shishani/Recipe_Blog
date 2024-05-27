using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Recipe_Blog.Models;

public partial class Request
{
    public decimal Id { get; set; }
	[Display(Name = "Order Date")]
	public DateTimeOffset? Requestdate { get; set; }

    public decimal? RecipeId { get; set; }

    public decimal? UserId { get; set; }

    public decimal? Tax { get; set; }

    public virtual Recipe? Recipe { get; set; }

    public virtual User? User { get; set; }
}
