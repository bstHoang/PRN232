using System;
using System.Collections.Generic;

namespace _10API.Models;

public partial class Service
{
    public int ServiceId { get; set; }

    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<PetService> PetServices { get; set; } = new List<PetService>();
}
