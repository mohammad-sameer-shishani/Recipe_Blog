using System;
using System.Collections.Generic;

namespace Recipe_Blog.Models;

public partial class Gender
{
    public decimal Genderid { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
