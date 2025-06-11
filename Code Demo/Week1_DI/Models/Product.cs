using System;
using System.Collections.Generic;

namespace Week1_DI.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
