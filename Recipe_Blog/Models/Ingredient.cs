using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Recipe_Blog.Models;

public partial class Ingredient
{
    [Key]
    public decimal Id { get; set; }

    public string? Description { get; set; }

    public decimal? RecipeId { get; set; }

    public virtual Recipe? Recipe { get; set; }
}
