using Microsoft.AspNetCore.Identity;

namespace CompanyManage.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public int PositionId { get; set; }

        // Thuộc tính điều hướng
        public Department Department { get; set; }
        public Position Position { get; set; }
    }
}
