using System;
using System.Collections.Generic;

namespace givenAPI.Models;

public partial class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime? ReleaseDate { get; set; }
    public string? Description { get; set; }
    public string Language { get; set; } = null!;
    public int? ProducerId { get; set; }
    public int? DirectorId { get; set; }

    // Navigation
    public virtual Producer? Producer { get; set; }
    public virtual Director? Director { get; set; }
    public virtual ICollection<Star> Stars { get; set; } = new List<Star>();
    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
}
