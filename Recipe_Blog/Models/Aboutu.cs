using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipe_Blog.Models;

public partial class Aboutu
{
    public string? AboutusContent { get; set; }

    public string? AboutCreator { get; set; }

    public decimal Id { get; set; }

    public string? Ingpath { get; set; }
	[NotMapped]
	public IFormFile? ImageFile { get; set; }
}
