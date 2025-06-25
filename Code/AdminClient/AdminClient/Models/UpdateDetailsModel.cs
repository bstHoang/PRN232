using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AdminClient.Models
{
    public class UpdateDetailsModel
    {
        public string Email { get; set; }
        public string? Password { get; set; }
        public bool IsDisabled { get; set; }
        public string RoleName { get; set; }
        [BindNever]
        public List<string>? Roles { get; set; }
    }
}
