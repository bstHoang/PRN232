namespace _10API.DTOS
{
    public class PetDTO
    {
        public int PetId { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? OwnerId { get; set; }
    }
}
