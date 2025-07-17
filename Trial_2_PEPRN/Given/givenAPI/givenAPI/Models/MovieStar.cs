namespace givenAPI.Models
{
    public partial class MovieStar
    {
        public int MovieId { get; set; }
        public int StarId { get; set; }

        public virtual Movie Movie { get; set; } = null!;
        public virtual Star Star { get; set; } = null!;
    }
}
