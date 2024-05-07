using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MessagePack;
using Microsoft.EntityFrameworkCore;

namespace Recipe_Blog.Models;

public partial class Login
{

    public decimal Id { get; set; }
    [Required]
    [StringLength(100)]
    public string Email { get; set; } = null!;
    [Required]
    [StringLength(50)]
	public string Password { get; set; } = null!;
    [Required]
	[StringLength(50)]
	[Display(Name = "User Name")]
	public string UserName { get; set; } = null!;

    public decimal? UserId { get; set; }

    public virtual User? User { get; set; }
}
