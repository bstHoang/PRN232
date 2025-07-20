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
        // lay thông tin trong 1 bảng (read)
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
                    Dob = s.Dob.HasValue ? s.Dob.Value.ToDateTime(TimeOnly.MinValue) : null,
                    Description = s.Description,
                    Nationality = s.Nationality
                })
                .ToList();

            return Ok(stars);
        }
        // lấy thông tin của các bàng nhiều nhiều (read)
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
                Dob = star.Dob.HasValue ? star.Dob.Value.ToDateTime(TimeOnly.MinValue).ToString("yyyy-MM-dd HH:mm:ss") : null,
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
                    ProducerName = (string?)null,
                    DirectorName = (string?)null,
                    Genres = m.Genres.Select(g => g.Title.Trim()).ToList(),
                    Stars = m.Stars.Select(s => s.FullName).ToList(),
                    Genres1 = (List<string>?)null,
                    Stars1 = (List<string>?)null,
                    Genres2 = new List<string>(),
                    Stars2 = new List<string>()
                }).ToList()
            };

            return Ok(result);
        }
        //delete quan hệ nhiều nhiều 
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
        // lay thong tin trong 1 bang (read)
        [HttpGet]
        [Route("/api/Directors/GetDirectors")]
        public IActionResult GetDirectors()
        {
            var directors = _context.Directors
                .Select(d => new
                {
                    d.Id,
                    d.FullName
                }).ToList();

            return Ok(directors);
        }
        //lay quan he 1 nhiều (read)
        [HttpGet]
        [Route("/api/Movies/GetMovies")]
        public IActionResult GetMovies()
        {
            var movies = _context.Movies.Include(m => m.Director)
                                        .Include(m => m.Producer).AsEnumerable()
                .Select(m => new
                {
                    m.Id,
                    m.Title,
                    ReleaseDate = m.ReleaseDate?.ToString("yyyy-MM-dd"),
                    m.Description,
                    m.Language,
                    DirectorName = m.Director.FullName,
                    ProducerName = m.Producer?.Name
                }).ToList();

            return Ok(movies);
        }
        // lấy 1 nhiều 
        [HttpGet]
        [Route("/api/Movies/GetMoviesByDirectorId/{id}")]
        public IActionResult GetMoviesByDirectorId(int id)
        {
            var director = _context.Directors.FirstOrDefault(d => d.Id == id);
            if (director == null)
                return NotFound("Director not found.");

            var movies = _context.Movies.Include(m => m.Director)
                                        .Include(m => m.Producer).AsEnumerable()
                .Where(m => m.DirectorId == id)
                .Select(m => new
                {
                    m.Id,
                    m.Title,
                    ReleaseDate = m.ReleaseDate?.ToString("yyyy-MM-dd"),
                    m.Description,
                    m.Language,
                    DirectorName = m.Director.FullName,
                    ProducerName = m.Producer?.Name
                })
                .ToList();

            return Ok(movies);
        }
        //Create trong 1 bảng return số lượng bản ghi đã thêm
        [HttpPost]
        [Route("/api/director/create")]
        public IActionResult CreateDirector([FromBody] Director newDirector)
        {
            try
            {
                _context.Directors.Add(newDirector);
                int recordsAdded = _context.SaveChanges();
                return Ok(recordsAdded); // Số lượng bản ghi đã thêm
            }
            catch (Exception)
            {
                return Conflict("There is an error while adding."); // Lỗi thêm dữ liệu
            }
        }
        //update trong bảng nhiều nhiều
        [HttpPut]
        [Route("api/movie/updatemovieingenre/{genreId}/{oldMovieId}")]
        public IActionResult UpdateMovieInGenre(int genreId, int oldMovieId, [FromBody] int newMovieId)
        {
            try
            {
                // Find the genre with its movies
                var genre = _context.Genres
                    .Include(g => g.Movies)
                    .FirstOrDefault(g => g.Id == genreId);

                if (genre == null)
                {
                    return NotFound("The requested genre could not be found.");
                }

                // Check if the old movie exists in the genre's movie list
                var oldMovie = genre.Movies.FirstOrDefault(m => m.Id == oldMovieId);
                if (oldMovie == null)
                {
                    return NotFound("The requested movie could not be found in the list of movies of the requested genre.");
                }

                // Check if the new movie exists in the database
                var newMovie = _context.Movies.FirstOrDefault(m => m.Id == newMovieId);
                if (newMovie == null)
                {
                    return NotFound("The new movie could not be found in the database.");
                }

                // Remove the old movie and add the new movie
                genre.Movies.Remove(oldMovie);
                genre.Movies.Add(newMovie);

                // Save changes to the database
                _context.SaveChanges();

                return Ok("Movie updated successfully in the genre.");
            }
            catch (Exception)
            {
                return Conflict("An error occurred while updating the movie in the genre.");
            }
        }
        //create trong bảng nhiều nhiều
        [HttpPost]
        [Route("api/movie/addmovietogenre/{genreId}")]
        public IActionResult AddMovieToGenre(int genreId, [FromBody] int movieId)
        {
            try
            {
                // Tìm genre với danh sách phim
                var genre = _context.Genres
                    .Include(g => g.Movies)
                    .FirstOrDefault(g => g.Id == genreId);

                if (genre == null)
                {
                    return NotFound("Thể loại không tồn tại.");
                }

                // Kiểm tra xem movieId có tồn tại trong bảng Movies
                var movie = _context.Movies.FirstOrDefault(m => m.Id == movieId);
                if (movie == null)
                {
                    return NotFound("Phim không tồn tại trong cơ sở dữ liệu.");
                }

                // Kiểm tra xem movie đã có trong danh sách phim của genre chưa
                if (genre.Movies.Any(m => m.Id == movieId))
                {
                    return Conflict("Phim đã tồn tại trong thể loại này.");
                }

                // Thêm phim vào danh sách phim của genre
                genre.Movies.Add(movie);

                // Lưu thay đổi vào cơ sở dữ liệu
                _context.SaveChanges();

                return Ok("Thêm phim vào thể loại thành công.");
            }
            catch (Exception)
            {
                return Conflict("Đã xảy ra lỗi khi thêm phim vào thể loại.");
            }
        }

        [HttpPost]
        [Route("api/movie/createmovie")]
        public async Task<IActionResult> CreateMovie([FromBody] Movie movie)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrWhiteSpace(movie.Title) || string.IsNullOrWhiteSpace(movie.Language))
                {
                    return BadRequest("Tiêu đề và ngôn ngữ là bắt buộc.");
                }

                // Kiểm tra DirectorId (nếu có)
                if (movie.DirectorId.HasValue && !await _context.Directors.AnyAsync(d => d.Id == movie.DirectorId))
                {
                    return BadRequest("Đạo diễn không tồn tại.");
                }

                // Kiểm tra ProducerId (nếu có)
                if (movie.ProducerId.HasValue && !await _context.Producers.AnyAsync(p => p.Id == movie.ProducerId))
                {
                    return BadRequest("Nhà sản xuất không tồn tại.");
                }

                // Kiểm tra và xử lý Genres (nếu có)
                if (movie.Genres != null && movie.Genres.Any())
                {
                    var genreIds = movie.Genres.Select(g => g.Id).ToList();
                    var existingGenres = await _context.Genres
                        .Where(g => genreIds.Contains(g.Id))
                        .ToListAsync();
                    if (existingGenres.Count != genreIds.Count)
                    {
                        return BadRequest("Một hoặc nhiều thể loại không tồn tại.");
                    }
                    movie.Genres = existingGenres; // Gán lại danh sách Genres đã kiểm tra
                }
                else
                {
                    movie.Genres = new List<Genre>(); // Đảm bảo Genres không null
                }

                // Kiểm tra và xử lý Stars (nếu có)
                if (movie.Stars != null && movie.Stars.Any())
                {
                    var starIds = movie.Stars.Select(s => s.Id).ToList();
                    var existingStars = await _context.Stars
                        .Where(s => starIds.Contains(s.Id))
                        .ToListAsync();
                    if (existingStars.Count != starIds.Count)
                    {
                        return BadRequest("Một hoặc nhiều diễn viên không tồn tại.");
                    }
                    movie.Stars = existingStars; // Gán lại danh sách Stars đã kiểm tra
                }
                else
                {
                    movie.Stars = new List<Star>(); // Đảm bảo Stars không null
                }

                // Xóa navigation properties để tránh lỗi khi lưu
                movie.Director = null;
                movie.Producer = null;

                // Thêm Movie vào DbContext và lưu
                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();

                // Trả về thông tin phim đã tạo
                return Ok(new
                {
                    movie.Id,
                    movie.Title,
                    movie.ReleaseDate,
                    movie.Description,
                    movie.Language,
                    movie.DirectorId,
                    movie.ProducerId,
                    GenreIds = movie.Genres.Select(g => g.Id).ToList(),
                    StarIds = movie.Stars.Select(s => s.Id).ToList()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi tạo phim: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("api/movies/deletemovie/{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            try
            {
                // Tìm phim theo Id, bao gồm các quan hệ Genres và Stars
                var movie = await _context.Movies
                    .Include(m => m.Genres)
                    .Include(m => m.Stars)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (movie == null)
                {
                    return NotFound("Phim không tồn tại.");
                }

                // Xóa phim (các quan hệ nhiều-nhiều trong Genres và Stars sẽ tự động bị xóa do cascade delete hoặc cấu hình DB)
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();

                return Ok("Xóa phim thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi xóa phim: {ex.Message}");
            }
        }
    }
}

