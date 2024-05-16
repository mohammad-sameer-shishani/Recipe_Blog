using System;
using System.Collections.Generic;

namespace Recipe_Blog.Models;

public partial class Comment
{
    public decimal Id { get; set; }

    public string? Message { get; set; }

    public decimal? UserId { get; set; }

    public decimal? RecipeId { get; set; }

    public virtual Recipe? Recipe { get; set; }

    public virtual User? User { get; set; }
}
