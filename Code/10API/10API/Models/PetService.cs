using System;
using System.Collections.Generic;

namespace _10API.Models;

public partial class PetService
{
    public int PetId { get; set; }

    public int ServiceId { get; set; }

    public DateOnly ServiceDate { get; set; }

    public string? Note { get; set; }

    public virtual Pet Pet { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
