using System;
using System.Collections.Generic;

namespace LAB1.Models;

public partial class News
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;
}
