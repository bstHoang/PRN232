namespace CompanyManage.Models
{
    public class EditEmployeeModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int DepartmentId { get; set; }
        public int PositionId { get; set; }
    }
}
