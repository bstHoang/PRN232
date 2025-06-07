using System;
using System.Collections.Generic;

namespace HW2.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
