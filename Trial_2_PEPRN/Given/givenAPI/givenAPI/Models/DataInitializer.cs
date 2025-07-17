using givenAPI.Models;
using System;
using System.Collections.Generic;

namespace givenAPI.Models
{
    public static class DataInitializer
    {
        // 1. Directors
        public static List<Director> Directors = new List<Director>
        {
            new Director { Id = 1, FullName = "David Gordon Green", Male = true, Dob = new DateTime(1975,4,9), Nationality = "USA", Description = "David Gordon Green was born on April 9, 1975 in Little Rock, Arkansas, USA. He is a producer and director, known for Sát Nhân Halloween (2018), Halloween Kills (2021) and George Washington (2000)." },
            new Director { Id = 2, FullName = "Aaron Horvath", Male = true, Dob = new DateTime(1980,8,19), Nationality = "USA", Description = "Aaron Horvath was born on August 19, 1980 in California, USA. He is a producer and writer, known for Teen Titans (2018), Teen Titans Go! (2013) and Naruto." },
            new Director { Id = 4, FullName = "David Bruckner", Male = true, Dob = new DateTime(1981,8,19), Nationality = "England", Description = "David Bruckner is known for The Night House (2020), Nghi Lễ Tế Thần (2017) and Southbound (2015)." },
            new Director { Id = 5, FullName = "Mike Barker", Male = true, Dob = new DateTime(1965,11,29), Nationality = "England", Description = "Mike Barker was born on November 29, 1965 in England, UK. He is a director and producer, known for Fargo (2014) and Broadchurch (2013)." },
            new Director { Id = 6, FullName = "Joseph Kosinski", Male = true, Dob = new DateTime(1974,5,3), Nationality = "USA", Description = "Joseph Kosinski is a director whose uncompromising style has quickly made a mark..." }
        };

        // 2. Producers
        public static List<Producer> Producers = new List<Producer>
        {
            new Producer { Id = 1, Name = "Paramount Pictures" },
            new Producer { Id = 2, Name = "Disney Channel" },
            new Producer { Id = 3, Name = "Blumhouse Productions" },
            new Producer { Id = 4, Name = "Universal Pictures" },
            new Producer { Id = 5, Name = "Cinemundo" },
            new Producer { Id = 6, Name = "Illumination Entertainment" },
            new Producer { Id = 7, Name = "20th Century Studios" },
            new Producer { Id = 8, Name = "Made Up Stories" }
        };

        // 3. Stars
        public static List<Star> Stars = new List<Star>
        {
            new Star { Id = 1, FullName = "Jamie Lee Curtis", Male = false, Dob = new DateTime(1958,11,22), Description = "Jamie Lee Curtis was born on November 22, 1958...", Nationality = "USA" },
            new Star { Id = 2, FullName = "Andi Matichak", Male = false, Dob = new DateTime(1970,9,11), Description = "Andi Matichak is an American actress...", Nationality = "USA" },
            new Star { Id = 3, FullName = "James Jude Courtney", Male = true, Dob = new DateTime(1957,1,31), Description = "James Jude Courtney is known for...", Nationality = "USA" },
            new Star { Id = 4, FullName = "Chris Pratt", Male = true, Dob = new DateTime(1979,6,21), Description = "Chris Pratt is an American actor...", Nationality = "USA" },
            new Star { Id = 5, FullName = "Anya Taylor-Joy", Male = false, Dob = new DateTime(1996,4,16), Description = "Anya Taylor-Joy is a British-American actress...", Nationality = "USA" },
            new Star { Id = 6, FullName = "Charlie Day", Male = true, Dob = new DateTime(1976,2,9), Description = "Charles Peckham Day is known for...", Nationality = "USA" },
            new Star { Id = 7, FullName = "Odessa A’zion", Male = false, Dob = new DateTime(1984,10,1), Description = "Odessa A’zion is known for Hellraiser (2022)...", Nationality = "USA" },
            new Star { Id = 8, FullName = "Jamie Clayton", Male = false, Dob = new DateTime(1978,1,15), Description = "Jamie Clayton is known for...", Nationality = "USA" },
            new Star { Id = 9, FullName = "Leonardo DiCaprio", Male = true, Dob = new DateTime(1974,11,11), Description = "Leonardo Wilhelm DiCaprio...", Nationality = "USA" },
            new Star { Id = 10, FullName = "Jonah Hill", Male = true, Dob = new DateTime(1983,12,20), Description = "Jonah Hill is known for...", Nationality = "USA" }
        };

        // 4. Genres
        public static List<Genre> Genres = new List<Genre>
        {
            new Genre { Id = 1, Title = "Action" },
            new Genre { Id = 2, Title = "Drama" },
            new Genre { Id = 6, Title = "Adventure" },
            new Genre { Id = 7, Title = "Comedy" },
            new Genre { Id = 8, Title = "Animation" },
            new Genre { Id = 9, Title = "Fantasy" },
            new Genre { Id = 10, Title = "Sci-fi" },
            new Genre { Id = 11, Title = "Crime" },
            new Genre { Id = 12, Title = "Thriller" },
            new Genre { Id = 13, Title = "Family" },
            new Genre { Id = 14, Title = "Horror" }
        };

        // 5. Movies
        public static List<Movie> Movies = new List<Movie>
        {
            new Movie { Id = 2, Title = "Halloween Ends", ReleaseDate = new DateTime(2022,10,14), Description = "The saga of Michael Myers...", Language = "English", ProducerId = 3, DirectorId = 1 },
            new Movie { Id = 3, Title = "The Super Mario Bros. Movie", ReleaseDate = new DateTime(2023,4,7), Description = "A plumber named Mario...", Language = "English", ProducerId = 6, DirectorId = 2 },
            new Movie { Id = 5, Title = "Hellraiser", ReleaseDate = new DateTime(2022,10,7), Description = "A take on Clive Barker's...", Language = "English", ProducerId = 7, DirectorId = 4 },
            new Movie { Id = 6, Title = "Luckiest Girl Alive", ReleaseDate = new DateTime(2022,10,7), Description = "A woman in New York...", Language = "English", ProducerId = 8, DirectorId = 5 },
            new Movie { Id = 8, Title = "Broadchurch", ReleaseDate = new DateTime(2013,3,4), Description = "The murder of a young boy...", Language = "English", ProducerId = 7, DirectorId = 5 },
            new Movie { Id = 9, Title = "Top Gun: Maverick", ReleaseDate = new DateTime(2022,5,27), Description = "After thirty years, Maverick...", Language = "English",ProducerId = 1, DirectorId = 6 },
            new Movie { Id = 10, Title = "The Wolf of Wall Street", ReleaseDate = new DateTime(2014,1,11), Description = "Based on the true story...", Language = "English", ProducerId = 1, DirectorId = 5 }
        };

        // 6. MovieStars
        public static List<MovieStar> MovieStars = new List<MovieStar>
        {
            new MovieStar { MovieId = 2, StarId = 1 }, new MovieStar { MovieId = 2, StarId = 2 }, new MovieStar { MovieId = 2, StarId = 3 },
            new MovieStar { MovieId = 3, StarId = 4 }, new MovieStar { MovieId = 3, StarId = 5 }, new MovieStar { MovieId = 3, StarId = 6 },
            new MovieStar { MovieId = 5, StarId = 7 }, new MovieStar { MovieId = 5, StarId = 8 },
            new MovieStar { MovieId = 10, StarId = 9 }, new MovieStar { MovieId = 10, StarId = 10 }
        };

        // 7. MovieGenres
        public static List<MovieGenre> MovieGenres = new List<MovieGenre>
        {
            new MovieGenre { MovieId = 2, GenreId = 12 }, new MovieGenre { MovieId = 2, GenreId = 14 },
            new MovieGenre { MovieId = 3, GenreId = 6 }, new MovieGenre { MovieId = 3, GenreId = 7 }, new MovieGenre { MovieId = 3, GenreId = 8 }, new MovieGenre { MovieId = 3, GenreId = 9 }, new MovieGenre { MovieId = 3, GenreId = 10 },
            new MovieGenre { MovieId = 5, GenreId = 12 }, new MovieGenre { MovieId = 5, GenreId = 14 },
            new MovieGenre { MovieId = 6, GenreId = 2 }, new MovieGenre { MovieId = 6, GenreId = 12 },
            new MovieGenre { MovieId = 8, GenreId = 2 }, new MovieGenre { MovieId = 8, GenreId = 11 },
            new MovieGenre { MovieId = 9, GenreId = 1 }, new MovieGenre { MovieId = 9, GenreId = 2 },
            new MovieGenre { MovieId = 10, GenreId = 2 }, new MovieGenre { MovieId = 10, GenreId = 7 }, new MovieGenre { MovieId = 10, GenreId = 11 }
        };

        // 8. Static constructor to wire up navigation properties
        static DataInitializer()
        {
        }
    }
}
