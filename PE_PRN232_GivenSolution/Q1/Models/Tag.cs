using System;
using System.Collections.Generic;

namespace Q1.Models;

public partial class Tag
{
    public int TagId { get; set; }

    public string? TagName { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
