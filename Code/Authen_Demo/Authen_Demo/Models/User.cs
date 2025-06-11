using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Authen_Demo.Models;

public partial class User 
{
    public int UserId { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập tài khoản.")]
    public string Account { get; set; } = null!;
    [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
    public string Password { get; set; } = null!;

    public int? RoleId { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    public virtual Role? Role { get; set; }
}
