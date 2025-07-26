using Newtonsoft.Json;
using Project_FontEnd.Models;
using System.Net.Http.Headers;
using System.Text;

namespace Project_FontEnd.Services
{
    public class ApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
        public readonly string _baseUrl = "http://localhost:5555/api";

        public ApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }
        private void AttachBearerToken()
        {
            var token = _httpContextAccessor.HttpContext?.Session?.GetString("Token");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
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
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/news/getnews");
                if (!response.IsSuccessStatusCode)
                {
                    var content1 = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"GetAllNews failed: StatusCode={response.StatusCode}, Response={content1}");
                    return new List<NewsModel>();
                }

                var content = await response.Content.ReadAsStringAsync();
                var newsList = JsonConvert.DeserializeObject<List<NewsModel>>(content) ?? new List<NewsModel>();

                // Lấy username cho mỗi tin tức
                foreach (var news in newsList)
                {
                    var user = await GetUserById(news.CreateBy.ToString()); // Chuyển int thành string
                    news.CreatedByName = user.Username;
                }

                return newsList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetAllNews error: {ex.Message}");
                return new List<NewsModel>();
            }
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
            Console.WriteLine($"CreateNews request: Body={JsonConvert.SerializeObject(model)}");
            var response = await _httpClient.PostAsync($"{_baseUrl}/news/createnew", content);
            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"CreateNews failed: StatusCode={response.StatusCode}, Response={responseContent}");
            }
            return response;
        }
        // Get All Categories
        public async Task<List<CategoryModel>> GetAllCategories()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/Categories/GetAllCategories");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<CategoryModel>>(content);
        }

        // Get All Tags
        public async Task<List<TagModel>> GetAllTags()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/Tags/GetAllTags");
                if (!response.IsSuccessStatusCode)
                {
                    var content1 = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"GetAllTags failed: StatusCode={response.StatusCode}, Response={content1}");
                    return new List<TagModel>();
                }

                var content = await response.Content.ReadAsStringAsync();
                var tags = JsonConvert.DeserializeObject<List<TagModel>>(content);
                return tags ?? new List<TagModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetAllTags error: {ex.Message}");
                return new List<TagModel>();
            }
        }

        public async Task<List<int>> GetTagIdsFromNames(List<string> tagNames)
        {
            var tags = await GetAllTags();
            var tagIds = new List<int>();
            foreach (var tagName in tagNames ?? new List<string>())
            {
                var tag = tags.Find(t => t.Name.Equals(tagName, StringComparison.OrdinalIgnoreCase));
                if (tag != null)
                {
                    tagIds.Add(tag.Id);
                }
            }
            return tagIds;
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
        public async Task<HttpResponseMessage> UpdateNews(int id, object model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            Console.WriteLine($"UpdateNews request: ID={id}, Body={JsonConvert.SerializeObject(model)}");
            var response = await _httpClient.PutAsync($"{_baseUrl}/news/updatenew/{id}", content);
            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"UpdateNews failed: StatusCode={response.StatusCode}, Response={responseContent}");
            }
            return response;
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

        public async Task<UserModel> GetUserById(string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/auth/GetAccounts/{userId}");
                if (!response.IsSuccessStatusCode)
                {
                    var content1 = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"GetUserById failed: UserId={userId}, StatusCode={response.StatusCode}, Response={content1}");
                    return new UserModel { Id = userId, Username = "Unknown" };
                }

                var content = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserModel>(content);
                return user ?? new UserModel { Id = userId, Username = "Unknown" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetUserById error: UserId={userId}, Message={ex.Message}");
                return new UserModel { Id = userId, Username = "Unknown" };
            }
        }

        public async Task<List<NewsModel>> GetAllNewsForManager(string token , string? title = null)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"{_baseUrl}/news/all");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<NewsModel>>(content);
        }

        public async Task<List<NewsModel>> GetNewsByCategoryId(int categoryId)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/news/bycategoryid/{categoryId}");
            if (!response.IsSuccessStatusCode) return new List<NewsModel>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<NewsModel>>(json);
        }

        public async Task<List<TagModel1>> GetTopTagsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/Tags/GetTopTagsList");
                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"GetTopTagsAsync failed: StatusCode={response.StatusCode}, Response={content}");
                    return new List<TagModel1>();
                }

                var json = await response.Content.ReadAsStringAsync();
                var tags = JsonConvert.DeserializeObject<List<TagModel1>>(json);
                return tags ?? new List<TagModel1>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetTopTagsAsync error: {ex.Message}");
                return new List<TagModel1>();
            }
        }

        public async Task<List<NewsModel>> GetNewsByTagAsync(int tagId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/news/GetNewsByTag/{tagId}");
                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"GetNewsByTagAsync failed: StatusCode={response.StatusCode}, Response={content}");
                    return new List<NewsModel>();
                }

                var json = await response.Content.ReadAsStringAsync();
                var newsList = JsonConvert.DeserializeObject<List<NewsModel>>(json) ?? new List<NewsModel>();

                // Lấy username cho mỗi tin tức
                foreach (var news in newsList)
                {
                    var user = await GetUserById(news.CreateBy.ToString());
                    news.CreatedByName = user.Username;
                }

                return newsList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetNewsByTagAsync error: {ex.Message}");
                return new List<NewsModel>();
            }
        }

        // Get all categories
        public async Task<List<CategoryModel>> GetAllCategoriesAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/Categories/GetAllCategories");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<CategoryModel>>(content);
        }

        // Get category by ID
        public async Task<CategoryModel> GetCategoryByIdAsync(int id)
        {
            AttachBearerToken();
            var response = await _httpClient.GetAsync($"{_baseUrl}/Categories/GetAllCategory/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CategoryModel>(content);
        }

        public async Task<(CategoryModel Category, string ErrorMessage)> CreateCategoryAsync(CategoryModel category)
        {
            try
            {
                var token = _httpContextAccessor.HttpContext?.Session?.GetString("Token");
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_baseUrl}/Category/Create", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return (JsonConvert.DeserializeObject<CategoryModel>(responseContent), null);
                }
                else
                {
                    try
                    {
                        var errorObj = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        string errorMessage = errorObj.message?.ToString() ?? $"Lỗi API: {responseContent} (Status: {response.StatusCode})";
                        return (null, errorMessage);
                    }
                    catch
                    {
                        return (null, $"Lỗi API: {(string.IsNullOrEmpty(responseContent) ? $"Không có nội dung phản hồi (Status: {response.StatusCode})" : responseContent)}");
                    }
                }
            }
            catch (Exception ex)
            {
                return (null, $"Lỗi khi gọi API: {ex.Message}");
            }
        }


        // Update an existing category
        public async Task<CategoryModel> UpdateCategoryAsync(int id, CategoryModel category)
        {
            AttachBearerToken();
            var content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_baseUrl}/Category/Update/{id}", content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CategoryModel>(responseContent);
        }

        // Delete a category
        public async Task<string> DeleteCategoryAsync(int id)
        {
            AttachBearerToken();
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/Category/Delete/{id}");
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<dynamic>(responseContent).message;
        }
    }
}