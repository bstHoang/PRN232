namespace CompanyManage.Models
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public bool IsDisabled { get; set; }

        public List<string> Roles { get; set; }
    }
}
