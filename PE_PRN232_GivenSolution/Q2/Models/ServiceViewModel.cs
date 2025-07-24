namespace Q2.Models
{
    public class ServiceViewModel
    {
        public int Id { get; set; }
        public string RoomTitle { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string FeeType { get; set; }
        public decimal Amount { get; set; }
        public string PaymentDate { get; set; }
        public RoomViewModel Room { get; set; }
        public EmployeeViewModel Employee { get; set; }
    }
}
