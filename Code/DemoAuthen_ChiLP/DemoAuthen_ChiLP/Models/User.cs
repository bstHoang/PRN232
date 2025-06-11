using System;
using System.Collections.Generic;

namespace DemoAuthen_ChiLP.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Account { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? RoleId { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    public virtual Role? Role { get; set; }
}
