using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Recipe_Blog.Models;

public partial class Visa
{
    [Key]
    public decimal Id { get; set; }

    public long Cardnumber { get; set; }

    public byte Cvc { get; set; }

    public string Nameoncard { get; set; } = null!;

    public DateTime? Expdate { get; set; }

    public decimal? Amount { get; set; }

    public decimal? UserId { get; set; }

    public decimal? RequestId { get; set; }

    public virtual Request? Request { get; set; }

    public virtual User? User { get; set; }
}
