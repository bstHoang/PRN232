namespace AdminClient.Models
{
    public class UpdateDetailsModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsDisabled { get; set; }
        public string Role { get; set; }
    }
}
