namespace CompanyManage.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Thuộc tính điều hướng: danh sách nhân viên trong phòng ban
        public ICollection<ApplicationUser> Employees { get; set; }
    }
}
