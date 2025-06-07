using System;
using System.Collections.Generic;

namespace HW1.Models;

public partial class Subject
{
    public int SubjectId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
