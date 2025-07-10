using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Q2.Models;

namespace Q2.Controllers
{
    public class MoviesController : Controller
    {

        public string GetGivenBaseURL()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            string baseUrl = config["GivenAPIBaseUrl"];
            return baseUrl;
        }

        public async Task<IActionResult> Director_Movie(int? id)
        {
            string _baseUrl = GetGivenBaseURL();
            using (HttpClient client = new HttpClient())
            {
                // Get all directors
                var directorsResponse = await client.GetStringAsync($"{_baseUrl}/api/Directors/GetDirectors");
                var directors = JsonConvert.DeserializeObject<List<Director>>(directorsResponse);

                // Get movies (by director if id is provided)
                string moviesUrl = id == null
                    ? $"{_baseUrl}/api/Movies/GetMovies"
                    : $"{_baseUrl}/api/Movies/GetMoviesByDirectorId/{id}";

                var moviesResponse = await client.GetStringAsync(moviesUrl);
                var movies = JsonConvert.DeserializeObject<List<Movie>>(moviesResponse);

                ViewBag.Directors = directors;
                ViewBag.Movies = movies;
            }

            return View();
        }
    }
}
