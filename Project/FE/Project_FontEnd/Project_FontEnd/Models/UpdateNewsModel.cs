using System.ComponentModel.DataAnnotations;

namespace Project_FontEnd.Models
{
    public class UpdateNewsModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public List<int> TagIds { get; set; } = new List<int>();

        public bool Disable { get; set; }
    }
}
