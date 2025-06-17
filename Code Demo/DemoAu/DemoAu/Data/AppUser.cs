using Microsoft.AspNetCore.Identity;

namespace DemoAu.Data
{
    public class AppUser:IdentityUser
    {
        public string? Major { get; set; }
    }
}
