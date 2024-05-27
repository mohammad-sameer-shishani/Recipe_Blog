using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipe_Blog.Models;

public partial class Aboutu
{
	[Display(Name = "Content")]
	public string? AboutusContent { get; set; }
	[Display(Name = "Creator")]
	public string? AboutCreator { get; set; }

    public decimal Id { get; set; }
	[Display(Name = "Image Path")]
	public string? Ingpath { get; set; }
	[NotMapped]
	[Display(Name = "Hero Image")]
	public IFormFile? ImageFile { get; set; }
}
