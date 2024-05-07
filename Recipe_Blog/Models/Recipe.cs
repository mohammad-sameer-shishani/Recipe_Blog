using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MessagePack;

namespace Recipe_Blog.Models;

public partial class Recipe
{
    
    public decimal Id { get; set; }

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public DateTimeOffset? Creationdate { get; set; }

    public string? Name { get; set; }

    public decimal? UserId { get; set; }
    [Display(Name = "Category")]
    public decimal? CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual User? User { get; set; }
}
