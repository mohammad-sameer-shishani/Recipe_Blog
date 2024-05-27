using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Recipe_Blog.Models;

public partial class Contactu
{
	[Display(Name = "User Name")]
	public string? Username { get; set; }
	[Display(Name = "Email")]
	public string? Useremail { get; set; }
	
	public string? Subject { get; set; }

    public string? Message { get; set; }

    public decimal Id { get; set; }
}
