using Microsoft.AspNetCore.Identity;

namespace APIsWithAu.Models
{
    public class AppUser:IdentityUser
    {
        public string? Major { get; set; }
    }
}
