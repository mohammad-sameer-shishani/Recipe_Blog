using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipe_Blog.Models;

public partial class Homepage
{
    public string? NavbarTitle { get; set; }

    public string? Logo { get; set; }
	[NotMapped]
	public IFormFile? LogoImageFile { get; set; }
	public string? SupportPhoneNumber { get; set; }

    public string? HeroImg { get; set; }
	[NotMapped]
	public IFormFile? HeroImageFile { get; set; }
	public string? FooterName { get; set; }

    public string? FooterPhoneNumber { get; set; }

    public string? FooterEmail { get; set; }

    public string? Copyright { get; set; }

    public decimal Id { get; set; }
}
