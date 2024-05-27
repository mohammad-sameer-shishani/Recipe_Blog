using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Recipe_Blog.Models;

public partial class Visa
{
    public decimal Id { get; set; }
	[Display(Name = "Card Number")]

	public long Cardnumber { get; set; }

    public byte Cvc { get; set; }
	[Display(Name = "Name on Card")]
	public string Nameoncard { get; set; } = null!;

    public decimal? Amount { get; set; }

    public decimal? UserId { get; set; }
	[Display(Name = "Exp Date")]
	public string? Expdate { get; set; }

    public virtual User? User { get; set; }
}
