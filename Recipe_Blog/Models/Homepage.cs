using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipe_Blog.Models;

public partial class Homepage
{
	[Display(Name = "NavBar Title")]
	public string? NavbarTitle { get; set; }

    public string? Logo { get; set; }
	[NotMapped]
	[Display(Name = "Logo Image")]
	public IFormFile? LogoImageFile { get; set; }
	[Display(Name = "Support Team Phone")]
	public string? SupportPhoneNumber { get; set; }
	[Display(Name = "Hero Image Path")]
	public string? HeroImg { get; set; }
	[NotMapped]
	[Display(Name = "Hero Image")]
	public IFormFile? HeroImageFile { get; set; }
	[Display(Name = "Footer Letter")]
	public string? FooterName { get; set; }
	[Display(Name = "Footer Phone Number")]
	public string? FooterPhoneNumber { get; set; }
	[Display(Name = "Foooter Email")]
	public string? FooterEmail { get; set; }
	[Display(Name = "Copy Right")]

	public string? Copyright { get; set; }

    public decimal Id { get; set; }
}
