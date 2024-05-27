using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipe_Blog.Models;

public partial class Category
{
    public decimal Id { get; set; }

    public string? Name { get; set; }

    public string? Imgpath { get; set; }
	[NotMapped]
	public IFormFile? ImageFile { get; set; }

	public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
