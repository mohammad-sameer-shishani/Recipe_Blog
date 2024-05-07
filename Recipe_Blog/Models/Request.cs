﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MessagePack;
using Microsoft.EntityFrameworkCore;

namespace Recipe_Blog.Models;

public partial class Request
{
    
    public decimal Id { get; set; }

    public DateTimeOffset? Requestdate { get; set; }

    public decimal? RecipeId { get; set; }

    public decimal? UserId { get; set; }

    public virtual Recipe? Recipe { get; set; }

    public virtual User? User { get; set; }

    public virtual ICollection<Visa> Visas { get; set; } = new List<Visa>();
}
