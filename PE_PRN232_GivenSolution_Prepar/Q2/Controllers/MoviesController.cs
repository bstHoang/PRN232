using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Q2.Models;

namespace Q2.Controllers
{
    public class MoviesController : Controller
    {
        public async Task<IActionResult> Director_Movie(int? id)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string directorsUrl = Utilities.GetAbsoluteUrl("/api/Directors/GetDirectors");
                    var directorsResponse = await client.GetStringAsync(directorsUrl);
                    var directors = JsonSerializer.Deserialize<List<Director>>(directorsResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true 
                    });

                    string moviesUrl = id == null
                        ? Utilities.GetAbsoluteUrl("/api/Movies/GetMovies")
                        : Utilities.GetAbsoluteUrl($"/api/Movies/GetMoviesByDirectorId/{id}");

                    var moviesResponse = await client.GetStringAsync(moviesUrl);
                    var movies = JsonSerializer.Deserialize<List<Movie>>(moviesResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true 
                    });

                    ViewBag.Directors = directors ?? new List<Director>();
                    ViewBag.Movies = movies ?? new List<Movie>();

                    return View();
                }
                catch (HttpRequestException ex)
                {
                    ViewBag.Directors = new List<Director>();
                    ViewBag.Movies = new List<Movie>();
                    ViewBag.ErrorMessage = "can solve API data: " + ex.Message;
                    return View();
                }
                catch (JsonException ex)
                {
                    ViewBag.Directors = new List<Director>();
                    ViewBag.Movies = new List<Movie>();
                    ViewBag.ErrorMessage = "fail to solve json data: " + ex.Message;
                    return View();
                }
            }
        }

        public async Task<IActionResult> DeleteMovie(int movieId, int? directorId)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string deleteUrl = Utilities.GetAbsoluteUrl($"/api/Movies/DeleteMovie/{movieId}");
                    var response = await client.DeleteAsync(deleteUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Director_Movie", new { id = directorId });
                    }
                    else
                    {
                        ViewBag.ErrorMessage = $"Lỗi khi xóa phim: {response.ReasonPhrase}";
                        string directorsUrl = Utilities.GetAbsoluteUrl("/api/Directors/GetDirectors");
                        var directorsResponse = await client.GetStringAsync(directorsUrl);
                        var directors = JsonSerializer.Deserialize<List<Director>>(directorsResponse, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        string moviesUrl = directorId == null
                            ? Utilities.GetAbsoluteUrl("/api/Movies/GetMovies")
                            : Utilities.GetAbsoluteUrl($"/api/Movies/GetMoviesByDirectorId/{directorId}");
                        var moviesResponse = await client.GetStringAsync(moviesUrl);
                        var movies = JsonSerializer.Deserialize<List<Movie>>(moviesResponse, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        ViewBag.Directors = directors ?? new List<Director>();
                        ViewBag.Movies = movies ?? new List<Movie>();
                        ViewBag.DirectorId = directorId;
                        return View("Director_Movie");
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"Lỗi khi gọi API: {ex.Message}";
                    string directorsUrl = Utilities.GetAbsoluteUrl("/api/Directors/GetDirectors");
                    var directorsResponse = await client.GetStringAsync(directorsUrl);
                    var directors = JsonSerializer.Deserialize<List<Director>>(directorsResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    string moviesUrl = directorId == null
                        ? Utilities.GetAbsoluteUrl("/api/Movies/GetMovies")
                        : Utilities.GetAbsoluteUrl($"/api/Movies/GetMoviesByDirectorId/{directorId}");
                    var moviesResponse = await client.GetStringAsync(moviesUrl);
                    var movies = JsonSerializer.Deserialize<List<Movie>>(moviesResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    ViewBag.Directors = directors ?? new List<Director>();
                    ViewBag.Movies = movies ?? new List<Movie>();
                    ViewBag.DirectorId = directorId;
                    return View("Director_Movie");
                }
            }
        }


        //[HttpGet]
        //public async Task<IActionResult> CreateMovie()
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        try
        //        {
        //            // Lấy danh sách đạo diễn
        //            string directorsUrl = Utilities.GetAbsoluteUrl("/api/Directors/GetDirectors");
        //            var directorsResponse = await client.GetStringAsync(directorsUrl);
        //            var directors = JsonSerializer.Deserialize<List<Director>>(directorsResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            // Lấy danh sách nhà sản xuất (giả định có API /api/Producers/GetProducers)
        //            string producersUrl = Utilities.GetAbsoluteUrl("/api/Producers/GetProducers");
        //            var producersResponse = await client.GetStringAsync(producersUrl);
        //            var producers = JsonSerializer.Deserialize<List<Producer>>(producersResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            // Lấy danh sách thể loại (giả định có API /api/Genres/GetGenres)
        //            string genresUrl = Utilities.GetAbsoluteUrl("/api/Genres/GetGenres");
        //            var genresResponse = await client.GetStringAsync(genresUrl);
        //            var genres = JsonSerializer.Deserialize<List<Genre>>(genresResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            // Lấy danh sách diễn viên (giả định có API /api/Stars/GetStars)
        //            string starsUrl = Utilities.GetAbsoluteUrl("/api/Stars/GetStars");
        //            var starsResponse = await client.GetStringAsync(starsUrl);
        //            var stars = JsonSerializer.Deserialize<List<Star>>(starsResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            // Gán dữ liệu vào ViewBag
        //            ViewBag.Directors = directors ?? new List<Director>();
        //            ViewBag.Producers = producers ?? new List<Producer>();
        //            ViewBag.Genres = genres ?? new List<Genre>();
        //            ViewBag.Stars = stars ?? new List<Star>();

        //            return View(new Movie());
        //        }
        //        catch (Exception ex)
        //        {
        //            ViewBag.ErrorMessage = $"Lỗi khi lấy dữ liệu: {ex.Message}";
        //            ViewBag.Directors = new List<Director>();
        //            ViewBag.Producers = new List<Producer>();
        //            ViewBag.Genres = new List<Genre>();
        //            ViewBag.Stars = new List<Star>();
        //            return View(new Movie());
        //        }
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> CreateMovie(Movie movie, int[] GenreIds, int[] StarIds)
        //{
        //    if (!ModelState.IsValid || string.IsNullOrWhiteSpace(movie.Title) || string.IsNullOrWhiteSpace(movie.Language))
        //    {
        //        ViewBag.ErrorMessage = "Tiêu đề và ngôn ngữ là bắt buộc.";
        //        // Lấy lại dữ liệu cho dropdown
        //        using (HttpClient client = new HttpClient())
        //        {
        //            string directorsUrl = Utilities.GetAbsoluteUrl("/api/Directors/GetDirectors");
        //            var directorsResponse = await client.GetStringAsync(directorsUrl);
        //            var directors = JsonSerializer.Deserialize<List<Director>>(directorsResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            string producersUrl = Utilities.GetAbsoluteUrl("/api/Producers/GetProducers");
        //            var producersResponse = await client.GetStringAsync(producersUrl);
        //            var producers = JsonSerializer.Deserialize<List<Producer>>(producersResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            string genresUrl = Utilities.GetAbsoluteUrl("/api/Genres/GetGenres");
        //            var genresResponse = await client.GetStringAsync(genresUrl);
        //            var genres = JsonSerializer.Deserialize<List<Genre>>(genresResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            string starsUrl = Utilities.GetAbsoluteUrl("/api/Stars/GetStars");
        //            var starsResponse = await client.GetStringAsync(starsUrl);
        //            var stars = JsonSerializer.Deserialize<List<Star>>(starsResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            ViewBag.Directors = directors ?? new List<Director>();
        //            ViewBag.Producers = producers ?? new List<Producer>();
        //            ViewBag.Genres = genres ?? new List<Genre>();
        //            ViewBag.Stars = stars ?? new List<Star>();
        //            return View(movie);
        //        }
        //    }

        //    using (HttpClient client = new HttpClient())
        //    {
        //        try
        //        {
        //            // Tạo dữ liệu gửi đến API
        //            var movieData = new
        //            {
        //                movie.Title,
        //                ReleaseDate = movie.ReleaseDate.ToString("yyyy-MM-dd"),
        //                movie.Description,
        //                movie.Language,
        //                movie.DirectorId,
        //                movie.ProducerId,
        //                Genres = GenreIds.Select(id => new { Id = id }).ToList(),
        //                Stars = StarIds.Select(id => new { Id = id }).ToList()
        //            };

        //            string createUrl = Utilities.GetAbsoluteUrl("/api/Movies/CreateMovie");
        //            var content = new StringContent(
        //                JsonSerializer.Serialize(movieData),
        //                System.Text.Encoding.UTF8,
        //                "application/json"
        //            );

        //            var response = await client.PostAsync(createUrl, content);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                return RedirectToAction("Director_Movie");
        //            }
        //            else
        //            {
        //                ViewBag.ErrorMessage = $"Lỗi khi tạo phim: {response.ReasonPhrase}";
        //                // Lấy lại dữ liệu cho dropdown
        //                string directorsUrl = Utilities.GetAbsoluteUrl("/api/Directors/GetDirectors");
        //                var directorsResponse = await client.GetStringAsync(directorsUrl);
        //                var directors = JsonSerializer.Deserialize<List<Director>>(directorsResponse, new JsonSerializerOptions
        //                {
        //                    PropertyNameCaseInsensitive = true
        //                });

        //                string producersUrl = Utilities.GetAbsoluteUrl("/api/Producers/GetProducers");
        //                var producersResponse = await client.GetStringAsync(producersUrl);
        //                var producers = JsonSerializer.Deserialize<List<Producer>>(producersResponse, new JsonSerializerOptions
        //                {
        //                    PropertyNameCaseInsensitive = true
        //                });

        //                string genresUrl = Utilities.GetAbsoluteUrl("/api/Genres/GetGenres");
        //                var genresResponse = await client.GetStringAsync(genresUrl);
        //                var genres = JsonSerializer.Deserialize<List<Genre>>(genresResponse, new JsonSerializerOptions
        //                {
        //                    PropertyNameCaseInsensitive = true
        //                });

        //                string starsUrl = Utilities.GetAbsoluteUrl("/api/Stars/GetStars");
        //                var starsResponse = await client.GetStringAsync(starsUrl);
        //                var stars = JsonSerializer.Deserialize<List<Star>>(starsResponse, new JsonSerializerOptions
        //                {
        //                    PropertyNameCaseInsensitive = true
        //                });

        //                ViewBag.Directors = directors ?? new List<Director>();
        //                ViewBag.Producers = producers ?? new List<Producer>();
        //                ViewBag.Genres = genres ?? new List<Genre>();
        //                ViewBag.Stars = stars ?? new List<Star>();
        //                return View(movie);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ViewBag.ErrorMessage = $"Lỗi khi gọi API: {ex.Message}";
        //            // Lấy lại dữ liệu cho dropdown
        //            string directorsUrl = Utilities.GetAbsoluteUrl("/api/Directors/GetDirectors");
        //            var directorsResponse = await client.GetStringAsync(directorsUrl);
        //            var directors = JsonSerializer.Deserialize<List<Director>>(directorsResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            string producersUrl = Utilities.GetAbsoluteUrl("/api/Producers/GetProducers");
        //            var producersResponse = await client.GetStringAsync(producersUrl);
        //            var producers = JsonSerializer.Deserialize<List<Producer>>(producersResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            string genresUrl = Utilities.GetAbsoluteUrl("/api/Genres/GetGenres");
        //            var genresResponse = await client.GetStringAsync(genresUrl);
        //            var genres = JsonSerializer.Deserialize<List<Genre>>(genresResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            string starsUrl = Utilities.GetAbsoluteUrl("/api/Stars/GetStars");
        //            var starsResponse = await client.GetStringAsync(starsUrl);
        //            var stars = JsonSerializer.Deserialize<List<Star>>(starsResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            ViewBag.Directors = directors ?? new List<Director>();
        //            ViewBag.Producers = producers ?? new List<Producer>();
        //            ViewBag.Genres = genres ?? new List<Genre>();
        //            ViewBag.Stars = stars ?? new List<Star>();
        //            return View(movie);
        //        }
        //    }
        //}

        //[HttpGet]
        //public async Task<IActionResult> EditMovie(int movieId, int? directorId)
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        try
        //        {
        //            // Lấy thông tin phim
        //            string movieUrl = Utilities.GetAbsoluteUrl($"/api/Movies/GetMovie/{movieId}");
        //            var movieResponse = await client.GetStringAsync(movieUrl);
        //            var movie = JsonSerializer.Deserialize<Movie>(movieResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            if (movie == null)
        //            {
        //                ViewBag.ErrorMessage = "Phim không tồn tại.";
        //                return RedirectToAction("Director_Movie", new { id = directorId });
        //            }

        //            // Lấy danh sách đạo diễn
        //            string directorsUrl = Utilities.GetAbsoluteUrl("/api/Directors/GetDirectors");
        //            var directorsResponse = await client.GetStringAsync(directorsUrl);
        //            var directors = JsonSerializer.Deserialize<List<Director>>(directorsResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            // Lấy danh sách nhà sản xuất
        //            string producersUrl = Utilities.GetAbsoluteUrl("/api/Producers/GetProducers");
        //            var producersResponse = await client.GetStringAsync(producersUrl);
        //            var producers = JsonSerializer.Deserialize<List<Producer>>(producersResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            // Lấy danh sách thể loại
        //            string genresUrl = Utilities.GetAbsoluteUrl("/api/Genres/GetGenres");
        //            var genresResponse = await client.GetStringAsync(genresUrl);
        //            var genres = JsonSerializer.Deserialize<List<Genre>>(genresResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            // Lấy danh sách diễn viên
        //            string starsUrl = Utilities.GetAbsoluteUrl("/api/Stars/GetStars");
        //            var starsResponse = await client.GetStringAsync(starsUrl);
        //            var stars = JsonSerializer.Deserialize<List<Star>>(starsResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            // Lấy danh sách GenreIds và StarIds hiện tại của phim
        //            string movieGenresUrl = Utilities.GetAbsoluteUrl($"/api/Movies/GetMovieGenres/{movieId}");
        //            var movieGenresResponse = await client.GetStringAsync(movieGenresUrl);
        //            var movieGenres = JsonSerializer.Deserialize<List<Genre>>(movieGenresResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            string movieStarsUrl = Utilities.GetAbsoluteUrl($"/api/Movies/GetMovieStars/{movieId}");
        //            var movieStarsResponse = await client.GetStringAsync(movieStarsUrl);
        //            var movieStars = JsonSerializer.Deserialize<List<Star>>(movieStarsResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            ViewBag.Directors = directors ?? new List<Director>();
        //            ViewBag.Producers = producers ?? new List<Producer>();
        //            ViewBag.Genres = genres ?? new List<Genre>();
        //            ViewBag.Stars = stars ?? new List<Star>();
        //            ViewBag.SelectedGenreIds = movieGenres?.Select(g => g.Id).ToList() ?? new List<int>();
        //            ViewBag.SelectedStarIds = movieStars?.Select(s => s.Id).ToList() ?? new List<int>();
        //            ViewBag.DirectorId = directorId;

        //            return View(movie);
        //        }
        //        catch (Exception ex)
        //        {
        //            ViewBag.ErrorMessage = $"Lỗi khi lấy dữ liệu: {ex.Message}";
        //            ViewBag.Directors = new List<Director>();
        //            ViewBag.Producers = new List<Producer>();
        //            ViewBag.Genres = new List<Genre>();
        //            ViewBag.Stars = new List<Star>();
        //            ViewBag.SelectedGenreIds = new List<int>();
        //            ViewBag.SelectedStarIds = new List<int>();
        //            ViewBag.DirectorId = directorId;
        //            return View(new Movie());
        //        }
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> EditMovie(Movie movie, int[] GenreIds, int[] StarIds, int? DirectorId)
        //{
        //    if (!ModelState.IsValid || string.IsNullOrWhiteSpace(movie.Title) || string.IsNullOrWhiteSpace(movie.Language))
        //    {
        //        ViewBag.ErrorMessage = "Tiêu đề và ngôn ngữ là bắt buộc.";
        //        using (HttpClient client = new HttpClient())
        //        {
        //            string directorsUrl = Utilities.GetAbsoluteUrl("/api/Directors/GetDirectors");
        //            var directorsResponse = await client.GetStringAsync(directorsUrl);
        //            var directors = JsonSerializer.Deserialize<List<Director>>(directorsResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            string producersUrl = Utilities.GetAbsoluteUrl("/api/Producers/GetProducers");
        //            var producersResponse = await client.GetStringAsync(producersUrl);
        //            var producers = JsonSerializer.Deserialize<List<Producer>>(producersResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            string genresUrl = Utilities.GetAbsoluteUrl("/api/Genres/GetGenres");
        //            var genresResponse = await client.GetStringAsync(genresUrl);
        //            var genres = JsonSerializer.Deserialize<List<Genre>>(genresResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            string starsUrl = Utilities.GetAbsoluteUrl("/api/Stars/GetStars");
        //            var starsResponse = await client.GetStringAsync(starsUrl);
        //            var stars = JsonSerializer.Deserialize<List<Star>>(starsResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            string movieGenresUrl = Utilities.GetAbsoluteUrl($"/api/Movies/GetMovieGenres/{movie.Id}");
        //            var movieGenresResponse = await client.GetStringAsync(movieGenresUrl);
        //            var movieGenres = JsonSerializer.Deserialize<List<Genre>>(movieGenresResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            string movieStarsUrl = Utilities.GetAbsoluteUrl($"/api/Movies/GetMovieStars/{movie.Id}");
        //            var movieStarsResponse = await client.GetStringAsync(movieStarsUrl);
        //            var movieStars = JsonSerializer.Deserialize<List<Star>>(movieStarsResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            ViewBag.Directors = directors ?? new List<Director>();
        //            ViewBag.Producers = producers ?? new List<Producer>();
        //            ViewBag.Genres = genres ?? new List<Genre>();
        //            ViewBag.Stars = stars ?? new List<Star>();
        //            ViewBag.SelectedGenreIds = movieGenres?.Select(g => g.Id).ToList() ?? new List<int>();
        //            ViewBag.SelectedStarIds = movieStars?.Select(s => s.Id).ToList() ?? new List<int>();
        //            ViewBag.DirectorId = DirectorId;
        //            return View(movie);
        //        }
        //    }

        //    using (HttpClient client = new HttpClient())
        //    {
        //        try
        //        {
        //            var movieData = new
        //            {
        //                movie.Id,
        //                movie.Title,
        //                ReleaseDate = movie.ReleaseDate?.ToString("yyyy-MM-dd"),
        //                movie.Description,
        //                movie.Language,
        //                movie.DirectorId,
        //                movie.ProducerId,
        //                Genres = GenreIds.Select(id => new { Id = id }).ToList(),
        //                Stars = StarIds.Select(id => new { Id = id }).ToList()
        //            };

        //            string updateUrl = Utilities.GetAbsoluteUrl($"/api/Movies/UpdateMovie/{movie.Id}");
        //            var content = new StringContent(
        //                JsonSerializer.Serialize(movieData),
        //                System.Text.Encoding.UTF8,
        //                "application/json"
        //            );

        //            var response = await client.PutAsync(updateUrl, content);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                return RedirectToAction("Director_Movie", new { id = DirectorId });
        //            }
        //            else
        //            {
        //                ViewBag.ErrorMessage = $"Lỗi khi cập nhật phim: {response.ReasonPhrase}";
        //                string directorsUrl = Utilities.GetAbsoluteUrl("/api/Directors/GetDirectors");
        //                var directorsResponse = await client.GetStringAsync(directorsUrl);
        //                var directors = JsonSerializer.Deserialize<List<Director>>(directorsResponse, new JsonSerializerOptions
        //                {
        //                    PropertyNameCaseInsensitive = true
        //                });

        //                string producersUrl = Utilities.GetAbsoluteUrl("/api/Producers/GetProducers");
        //                var producersResponse = await client.GetStringAsync(producersUrl);
        //                var producers = JsonSerializer.Deserialize<List<Producer>>(producersResponse, new JsonSerializerOptions
        //                {
        //                    PropertyNameCaseInsensitive = true
        //                });

        //                string genresUrl = Utilities.GetAbsoluteUrl("/api/Genres/GetGenres");
        //                var genresResponse = await client.GetStringAsync(genresUrl);
        //                var genres = JsonSerializer.Deserialize<List<Genre>>(genresResponse, new JsonSerializerOptions
        //                {
        //                    PropertyNameCaseInsensitive = true
        //                });

        //                string starsUrl = Utilities.GetAbsoluteUrl("/api/Stars/GetStars");
        //                var starsResponse = await client.GetStringAsync(starsUrl);
        //                var stars = JsonSerializer.Deserialize<List<Star>>(starsResponse, new JsonSerializerOptions
        //                {
        //                    PropertyNameCaseInsensitive = true
        //                });

        //                string movieGenresUrl = Utilities.GetAbsoluteUrl($"/api/Movies/GetMovieGenres/{movie.Id}");
        //                var movieGenresResponse = await client.GetStringAsync(movieGenresUrl);
        //                var movieGenres = JsonSerializer.Deserialize<List<Genre>>(movieGenresResponse, new JsonSerializerOptions
        //                {
        //                    PropertyNameCaseInsensitive = true
        //                });

        //                string movieStarsUrl = Utilities.GetAbsoluteUrl($"/api/Movies/GetMovieStars/{movie.Id}");
        //                var movieStarsResponse = await client.GetStringAsync(movieStarsUrl);
        //                var movieStars = JsonSerializer.Deserialize<List<Star>>(movieStarsResponse, new JsonSerializerOptions
        //                {
        //                    PropertyNameCaseInsensitive = true
        //                });

        //                ViewBag.Directors = directors ?? new List<Director>();
        //                ViewBag.Producers = producers ?? new List<Producer>();
        //                ViewBag.Genres = genres ?? new List<Genre>();
        //                ViewBag.Stars = stars ?? new List<Star>();
        //                ViewBag.SelectedGenreIds = movieGenres?.Select(g => g.Id).ToList() ?? new List<int>();
        //                ViewBag.SelectedStarIds = movieStars?.Select(s => s.Id).ToList() ?? new List<int>();
        //                ViewBag.DirectorId = DirectorId;
        //                return View(movie);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ViewBag.ErrorMessage = $"Lỗi khi gọi API: {ex.Message}";
        //            string directorsUrl = Utilities.GetAbsoluteUrl("/api/Directors/GetDirectors");
        //            var directorsResponse = await client.GetStringAsync(directorsUrl);
        //            var directors = JsonSerializer.Deserialize<List<Director>>(directorsResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            string producersUrl = Utilities.GetAbsoluteUrl("/api/Producers/GetProducers");
        //            var producersResponse = await client.GetStringAsync(producersUrl);
        //            var producers = JsonSerializer.Deserialize<List<Producer>>(producersResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            string genresUrl = Utilities.GetAbsoluteUrl("/api/Genres/GetGenres");
        //            var genresResponse = await client.GetStringAsync(genresUrl);
        //            var genres = JsonSerializer.Deserialize<List<Genre>>(genresResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            string starsUrl = Utilities.GetAbsoluteUrl("/api/Stars/GetStars");
        //            var starsResponse = await client.GetStringAsync(starsUrl);
        //            var stars = JsonSerializer.Deserialize<List<Star>>(starsResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            string movieGenresUrl = Utilities.GetAbsoluteUrl($"/api/Movies/GetMovieGenres/{movie.Id}");
        //            var movieGenresResponse = await client.GetStringAsync(movieGenresUrl);
        //            var movieGenres = JsonSerializer.Deserialize<List<Genre>>(movieGenresResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            string movieStarsUrl = Utilities.GetAbsoluteUrl($"/api/Movies/GetMovieStars/{movie.Id}");
        //            var movieStarsResponse = await client.GetStringAsync(movieStarsUrl);
        //            var movieStars = JsonSerializer.Deserialize<List<Star>>(movieStarsResponse, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            ViewBag.Directors = directors ?? new List<Director>();
        //            ViewBag.Producers = producers ?? new List<Producer>();
        //            ViewBag.Genres = genres ?? new List<Genre>();
        //            ViewBag.Stars = stars ?? new List<Star>();
        //            ViewBag.SelectedGenreIds = movieGenres?.Select(g => g.Id).ToList() ?? new List<int>();
        //            ViewBag.SelectedStarIds = movieStars?.Select(s => s.Id).ToList() ?? new List<int>();
        //            ViewBag.DirectorId = DirectorId;
        //            return View(movie);
        //        }
        //    }
        //}
    }
}