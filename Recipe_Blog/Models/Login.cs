using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Recipe_Blog.Models;

public partial class Login
{
    public decimal Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
	[Display(Name = "User name")]
	public string UserName { get; set; } = null!;

    public decimal? UserId { get; set; }

    public virtual User? User { get; set; }
}
