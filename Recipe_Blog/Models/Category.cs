using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Recipe_Blog.Models;

public partial class Category
{
    [Key]
    public decimal Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
