using System;
using System.Collections.Generic;

namespace Q1.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
