using Microsoft.AspNetCore.Identity;

namespace Project.Models
{
    public class ApplicationUser : IdentityUser<int> 
    {
        public string Email { get; set; } 
        public int RoleId { get; set; }   
    }

}
