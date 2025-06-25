using System;
using System.Collections.Generic;

namespace _10API.Models;

public partial class Pet
{
    public int PetId { get; set; }

    public string? Name { get; set; }

    public string? Type { get; set; }

    public DateOnly? BirthDate { get; set; }

    public int? OwnerId { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Owner? Owner { get; set; }

    public virtual ICollection<PetService> PetServices { get; set; } = new List<PetService>();
}
