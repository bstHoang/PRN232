namespace CompanyManage.Models
{
    public class Position
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Thuộc tính điều hướng: danh sách nhân viên có vị trí này
        public ICollection<ApplicationUser> Employees { get; set; }
    }
}
