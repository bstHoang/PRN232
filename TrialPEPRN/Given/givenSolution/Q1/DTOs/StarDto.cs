namespace Q1.DTOs
{
    public class StarDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public bool Male { get; set; }
        public string Gender => Male ? "Male" : "Female";
        public DateOnly? Dob { get; set; } 
        public string DobString => Dob?.ToString("M/d/yyyy") ?? "";
        public string Description { get; set; }
        public string Nationality { get; set; }
    }
}
