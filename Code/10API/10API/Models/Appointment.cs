using System;
using System.Collections.Generic;

namespace _10API.Models;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int? PetId { get; set; }

    public DateTime? AppointmentDate { get; set; }

    public string? Reason { get; set; }

    public virtual Pet? Pet { get; set; }
}
