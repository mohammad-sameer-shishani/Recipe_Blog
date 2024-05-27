using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipe_Blog.Models;

public partial class Category
{
    public decimal Id { get; set; }
	[Display(Name = "Category Name")]
	public string? Name { get; set; }
	[Display(Name = "Image Path")]
	public string? Imgpath { get; set; }
	[NotMapped]
	[Display(Name = "Category Image")]
	public IFormFile? ImageFile { get; set; }

	public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
