namespace AdminClient.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public int? DepartmentId { get; set; }
        public int? PositionId { get; set; }
        public bool IsDisabled { get; set; }
    }
}
