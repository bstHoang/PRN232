using Newtonsoft.Json;
using Project_FontEnd.Models;
using System.Text;

namespace Project_FontEnd.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        public readonly string _baseUrl = "http://localhost:5555/api";

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Register
        public async Task<HttpResponseMessage> Register(RegisterModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync($"{_baseUrl}/auth/register", content);
        }

        // Verify Email
        public async Task<HttpResponseMessage> Verify(VerifyModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync($"{_baseUrl}/auth/verify", content);
        }

        // Login
        public async Task<HttpResponseMessage> Login(LoginModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync($"{_baseUrl}/auth/login", content);
        }

        // Get All News
        public async Task<List<NewsModel>> GetAllNews()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/news/getnews");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<NewsModel>>(content);
        }

        // Search News by Title
        public async Task<List<NewsModel>> SearchNews(string title)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/news/search?title={title}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<NewsModel>>(content);
        }

        // Get News by Id
        public async Task<NewsModel> GetNewsById(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/news/getnew/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<NewsModel>(content);
        }

        // Create News
        public async Task<HttpResponseMessage> CreateNews(CreateNewsModel model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync($"{_baseUrl}/news/createnew", content);
        }

        // Get My News
        public async Task<List<NewsModel>> GetMyNews(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"{_baseUrl}/news/mynews");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<NewsModel>>(content);
        }

        // Update News
        public async Task<HttpResponseMessage> UpdateNews(int id, CreateNewsModel model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            return await _httpClient.PutAsync($"{_baseUrl}/news/updatenew/{id}", content);
        }

        // Delete News
        public async Task<HttpResponseMessage> DeleteNews(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.DeleteAsync($"{_baseUrl}/news/delete/{id}");
        }

        public async Task<bool> ResendCode(string email)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/auth/resendcode", new { Email = email });
            return response.IsSuccessStatusCode;
        }

        public async Task<string> GetErrorMessageAsync(string url, object body)
        {
            var response = await _httpClient.PostAsJsonAsync(url, body);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                try
                {
                    var errorObj = JsonConvert.DeserializeObject<dynamic>(content);
                    return errorObj?.message?.ToString() ?? $"HTTP {response.StatusCode}: {response.ReasonPhrase}";
                }
                catch
                {
                    return $"HTTP {response.StatusCode}: {response.ReasonPhrase}";
                }
            }
            return string.Empty;
        }
       
    }
}