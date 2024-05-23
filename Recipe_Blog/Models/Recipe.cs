using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipe_Blog.Models;

public partial class Recipe
{
    public decimal Id { get; set; }

    public string? Description { get; set; }

    public DateTimeOffset? Creationdate { get; set; }

    public string? Name { get; set; }

    public decimal? UserId { get; set; }

    public decimal? CategoryId { get; set; }

    public decimal? RecipeStatusId { get; set; }

    public string? Imgpath { get; set; }
    [NotMapped]
    public IFormFile? ImageFile { get; set; }

    public string? Ingredients { get; set; }

    public string? Instructions { get; set; }

    public decimal? Price { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Status? RecipeStatus { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual User? User { get; set; }
}
