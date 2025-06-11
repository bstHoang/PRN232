using System;
using System.Collections.Generic;

namespace Authen_Demo.Models;

public partial class Grade
{
    public int GradeId { get; set; }

    public int? UserId { get; set; }

    public int? SubjectId { get; set; }

    public string? GradeType { get; set; }

    public decimal? GradeValue { get; set; }

    public decimal? Weight { get; set; }

    public virtual Subject? Subject { get; set; }

    public virtual User? User { get; set; }
}
