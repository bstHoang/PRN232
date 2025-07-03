using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Q1.DTOs;
using Q1.Models;

namespace Q1.Controllers
{
    public class StarController : Controller
    {
        private readonly PePrnFall22B1Context _context;

        public StarController(PePrnFall22B1Context context)
        {
            _context = context;
        }
        [EnableQuery]
        [HttpGet]
        [Route("api/star/getstars/{nationality}/{gender}")]
        public IActionResult GetStarsByNationalityAndGender(string nationality, string gender)
        {
            gender = gender.ToLower();
            bool isMale = gender == "male";

            var stars = _context.Stars
                .Where(s => s.Nationality == nationality && s.Male == isMale)
                .Select(s => new StarDto
                {
                    Id = s.Id,
                    FullName = s.FullName,
                    Male = s.Male ?? true, 
                    Dob = s.Dob,
                    Description = s.Description,
                    Nationality = s.Nationality
                })
                .ToList();

            return Ok(stars);
        }

        [EnableQuery]
        [HttpGet]
        [Route("api/star/getstar/{id}")]
        public IActionResult GetStar(int id)
        {
            var star = _context.Stars
                .Include(s => s.Movies) 
                    .ThenInclude(m => m.Genres) 
                .Include(s => s.Movies)
                    .ThenInclude(m => m.Stars) 
                .Include(s => s.Movies)
                    .ThenInclude(m => m.Producer)
                .Include(s => s.Movies)
                    .ThenInclude(m => m.Director)
                .FirstOrDefault(s => s.Id == id);

            if (star == null) return NotFound();

            var result = new
            {
                star.Id,
                star.FullName,
                Gender = star.Male == true ? "Male" : "Female",
                Dob = star.Dob,
                DobString = star.Dob?.ToString("M/d/yyyy"),
                star.Nationality,
                star.Description,
                Movies = star.Movies.Select(m => new
                {
                    m.Id,
                    m.Title,
                    ReleaseDate = m.ReleaseDate,
                    ReleaseYear = m.ReleaseDate?.Year,
                    m.Description,
                    m.Language,
                    ProducerId = m.ProducerId,
                    DirectorId = m.DirectorId,
                    ProducerName = m.Producer?.Name,
                    DirectorName = m.Director?.FullName,
                    Genres = m.Genres.Select(g => g.Title.Trim()).ToList(),
                    Stars = m.Stars.Select(s => s.FullName).ToList()
                }).ToList()
            };

            return Ok(result);
        }

        [HttpDelete]
        [Route("api/movie/removemoviefromgenre/{genreId}/{movieId}")]
        public IActionResult RemoveMovieFromGenre(int genreId, int movieId)
        {
            try
            {
                var genre = _context.Genres
                    .Include(g => g.Movies)
                    .FirstOrDefault(g => g.Id == genreId);

                if (genre == null)
                {
                    return NotFound("The requested genre could not be found.");
                }

                var movie = genre.Movies.FirstOrDefault(m => m.Id == movieId);
                if (movie == null)
                {
                    return NotFound("The requested movie could not be found in the list of movies of the requested genre.");
                }

                genre.Movies.Remove(movie);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception)
            {
                return Conflict("An error occurred while removing the movie from the genre.");
            }
        }
    }
}
