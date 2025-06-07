﻿using System;
using System.Collections.Generic;

namespace B3.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int? Quantity { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsActive { get; set; }
}
