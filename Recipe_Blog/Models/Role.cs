using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Recipe_Blog.Models;

public partial class Role
{
    [Key]
    public decimal Roleid { get; set; }

    public string Rolename { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
