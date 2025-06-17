namespace CompanyManage.Models
{
    public class CreateEmployeeModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int DepartmentId { get; set; }
        public int PositionId { get; set; }
    }
}
