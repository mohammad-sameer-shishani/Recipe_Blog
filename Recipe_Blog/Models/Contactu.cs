using System;
using System.Collections.Generic;

namespace Recipe_Blog.Models;

public partial class Contactu
{
    public string? Username { get; set; }

    public string? Useremail { get; set; }

    public string? Subject { get; set; }

    public string? Message { get; set; }

    public decimal Id { get; set; }
}
