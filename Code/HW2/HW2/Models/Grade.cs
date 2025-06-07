using System;
using System.Collections.Generic;

namespace HW2.Models;

public partial class Grade
{
    public int GradeId { get; set; }

    public int? StudentId { get; set; }

    public int? SubjectId { get; set; }

    public string? GradeType { get; set; }

    public decimal? GradeValue { get; set; }

    public decimal? Weight { get; set; }

    public virtual Student? Student { get; set; }

    public virtual Subject? Subject { get; set; }
}
