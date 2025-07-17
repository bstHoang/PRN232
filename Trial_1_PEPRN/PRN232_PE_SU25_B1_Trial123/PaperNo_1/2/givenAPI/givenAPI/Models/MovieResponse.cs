// Models/Responses/MovieResponse.cs
using System;
using System.Collections.Generic;

namespace givenAPI.Models.Responses
{
    public class MovieResponse
    {

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime? ReleaseDate { get; set; }
        public string? Description { get; set; }
        public string Language { get; set; } = null!;
        public int? ProducerId { get; set; }
        public int? DirectorId { get; set; }


        public DirectorInfo? Director { get; set; }

 
        public List<StarInfo> Stars { get; set; } = new();


        public List<GenreInfo> Genres { get; set; } = new();

 
        public class DirectorInfo
        {
            public int Id { get; set; }
            public string FullName { get; set; } = null!;
            public bool Male { get; set; }
            public DateTime Dob { get; set; }
            public string Nationality { get; set; } = null!;
            public string Description { get; set; } = null!;
        }

        public class StarInfo
        {
            public int Id { get; set; }
            public string FullName { get; set; } = null!;
            public bool? Male { get; set; }
            public DateTime? Dob { get; set; }
            public string? Description { get; set; }
            public string? Nationality { get; set; }
        }

        public class GenreInfo
        {
            public int Id { get; set; }
            public string Title { get; set; } = null!;
        }
    }
}
