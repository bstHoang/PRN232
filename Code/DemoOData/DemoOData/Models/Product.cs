using System;
using System.Collections.Generic;

namespace DemoOData.Models;

public partial class Product
{
    public int Id { get; set; }

    public string? ProductName { get; set; }

    public string? Brand { get; set; }

    public decimal Cost { get; set; }

    public string? ImageName { get; set; }

    public string? Type { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
