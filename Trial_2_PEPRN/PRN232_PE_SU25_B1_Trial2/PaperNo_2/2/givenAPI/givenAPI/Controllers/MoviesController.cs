// Controllers/MoviesController.cs
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using givenAPI.Models;
using givenAPI.Models.Responses;

namespace givenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        // GET: api/Movies/GetMovies
        [HttpGet("GetMovies")]
        public ActionResult<List<MovieResponse>> GetMovies()
        {
            var movies = DataInitializer.Movies;
            var directors = DataInitializer.Directors;
            var stars = DataInitializer.Stars;
            var genres = DataInitializer.Genres;
            var movieStars = DataInitializer.MovieStars;
            var movieGenres = DataInitializer.MovieGenres;

            List<MovieResponse> result = movies
                .Select(m =>
                {
                    // tìm Director cho từng movie
                    var dir = directors.FirstOrDefault(d => d.Id == m.DirectorId);

                    return new MovieResponse
                    {
                        Id = m.Id,
                        Title = m.Title,
                        ReleaseDate = m.ReleaseDate,
                        Description = m.Description,
                        Language = m.Language,
                        ProducerId = m.ProducerId,
                        DirectorId = m.DirectorId,

                        Director = dir is null ? null : new MovieResponse.DirectorInfo
                        {
                            Id = dir.Id,
                            FullName = dir.FullName,
                            Male = dir.Male,
                            Dob = dir.Dob,
                            Nationality = dir.Nationality,
                            Description = dir.Description
                        },

                        Stars = movieStars
                            .Where(ms => ms.MovieId == m.Id)
                            .Select(ms =>
                            {
                                var s = stars.First(st => st.Id == ms.StarId);
                                return new MovieResponse.StarInfo
                                {
                                    Id = s.Id,
                                    FullName = s.FullName,
                                    Male = s.Male,
                                    Dob = s.Dob,
                                    Description = s.Description,
                                    Nationality = s.Nationality
                                };
                            })
                            .ToList(),

                        Genres = movieGenres
                            .Where(mg => mg.MovieId == m.Id)
                            .Select(mg =>
                            {
                                var g = genres.First(ge => ge.Id == mg.GenreId);
                                return new MovieResponse.GenreInfo
                                {
                                    Id = g.Id,
                                    Title = g.Title
                                };
                            })
                            .ToList()
                    };
                })
                .ToList();

            return Ok(result);
        }

        // GET: api/Movies/GetMoviesByDirectorId/{directorId}
        [HttpGet("GetMoviesByDirectorId/{directorId}")]
        public ActionResult<List<MovieResponse>> GetMoviesByDirectorId(int directorId)
        {
            var movies = DataInitializer.Movies.Where(m => m.DirectorId == directorId);
            var directors = DataInitializer.Directors;
            var stars = DataInitializer.Stars;
            var genres = DataInitializer.Genres;
            var movieStars = DataInitializer.MovieStars;
            var movieGenres = DataInitializer.MovieGenres;

            List<MovieResponse> result = movies
                .Select(m =>
                {
                    var dir = directors.FirstOrDefault(d => d.Id == m.DirectorId);

                    return new MovieResponse
                    {
                        Id = m.Id,
                        Title = m.Title,
                        ReleaseDate = m.ReleaseDate,
                        Description = m.Description,
                        Language = m.Language,
                        ProducerId = m.ProducerId,
                        DirectorId = m.DirectorId,

                        Director = dir is null ? null : new MovieResponse.DirectorInfo
                        {
                            Id = dir.Id,
                            FullName = dir.FullName,
                            Male = dir.Male,
                            Dob = dir.Dob,
                            Nationality = dir.Nationality,
                            Description = dir.Description
                        },

                        Stars = movieStars
                            .Where(ms => ms.MovieId == m.Id)
                            .Select(ms =>
                            {
                                var s = stars.First(st => st.Id == ms.StarId);
                                return new MovieResponse.StarInfo
                                {
                                    Id = s.Id,
                                    FullName = s.FullName,
                                    Male = s.Male,
                                    Dob = s.Dob,
                                    Description = s.Description,
                                    Nationality = s.Nationality
                                };
                            })
                            .ToList(),

                        Genres = movieGenres
                            .Where(mg => mg.MovieId == m.Id)
                            .Select(mg =>
                            {
                                var g = genres.First(ge => ge.Id == mg.GenreId);
                                return new MovieResponse.GenreInfo
                                {
                                    Id = g.Id,
                                    Title = g.Title
                                };
                            })
                            .ToList()
                    };
                })
                .ToList();

            return Ok(result);
        }

        // DELETE: api/Movies/DeleteMovie/{id}
        [HttpDelete("DeleteMovie/{id}")]
        public IActionResult DeleteMovie(int id)
        {
            var movie = DataInitializer.Movies.FirstOrDefault(m => m.Id == id);
            if (movie == null)
                return NotFound(new { Message = $"Không tìm thấy phim với Id = {id}." });

            DataInitializer.MovieStars.RemoveAll(ms => ms.MovieId == id);
            DataInitializer.MovieGenres.RemoveAll(mg => mg.MovieId == id);
            DataInitializer.Movies.Remove(movie);

            return NoContent();
        }
    }
}
