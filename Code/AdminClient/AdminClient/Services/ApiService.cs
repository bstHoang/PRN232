using System.Net.Http.Headers;
using System.Text;
using AdminClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AdminClient.Services
{
    public class ApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _apiBaseUrl = "http://localhost:5265";

        public ApiService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetToken()
        {
            return _httpContextAccessor.HttpContext.Session.GetString("JwtToken");
        }

        public async Task<string> LoginAsync(LoginModel model)
        {
            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{_apiBaseUrl}/api/AccountAPI/Login", content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(json);
                return result.token;
            }
            return null;
        }

        public async Task<List<EmployeeViewModel>> GetEmployeeListAsync(string filter = null)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
            var url = $"{_apiBaseUrl}/api/AdminAPI/ViewEmployeeList";
            if (!string.IsNullOrEmpty(filter))
                url += $"?$filter={Uri.EscapeDataString(filter)}";

            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<EmployeeViewModel>>(json);
        }

        public async Task<bool> SetCredentialsAsync(int id, SetCredentialsModel model)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"{_apiBaseUrl}/api/AdminAPI/CreateAccount/{id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAccountInfoAsync(int id, UpdateDetailsModel model)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var rawJson = JsonConvert.SerializeObject(model);
            System.Diagnostics.Debug.WriteLine("Sending JSON: " + rawJson);

            var response = await client.PutAsync($"{_apiBaseUrl}/api/AdminAPI/UpdateAccountInfo/{id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<DepartmentModel>> GetDepartmentsAsync()
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken()); // Implement GetToken() as needed
            var response = await client.GetAsync($"{_apiBaseUrl}/api/DepartmentsAPI");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<DepartmentModel>>(json);
        }

        public async Task<List<PositionModel>> GetPositionsAsync()
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken()); // Implement GetToken() as needed
            var response = await client.GetAsync($"{_apiBaseUrl}/api/PositionsAPI");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<PositionModel>>(json);
        }

        public async Task<List<string>> GetRolesAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var token = GetToken();
                if (string.IsNullOrEmpty(token))
                {
                    System.Diagnostics.Debug.WriteLine("No JWT token found");
                    return new List<string>();
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync($"{_apiBaseUrl}/api/RoleAPI");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"RoleAPI error: {response.StatusCode}, Content: {errorContent}");
                    return new List<string>();
                }

                var json = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"RoleAPI response: {json}");

                // Phân tích JSON thành danh sách đối tượng và lấy thuộc tính name
                var roleObjects = JsonConvert.DeserializeObject<List<dynamic>>(json);
                var roles = roleObjects.Select(r => (string)r.name).ToList();
                return roles ?? new List<string>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetRolesAsync: {ex.Message}");
                return new List<string>();
            }
        }

        public async Task<List<string>> GetUserRolesAsync(int userId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var token = GetToken();
                if (string.IsNullOrEmpty(token))
                {
                    System.Diagnostics.Debug.WriteLine("No JWT token found");
                    return new List<string>();
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync($"{_apiBaseUrl}/api/AdminAPI/GetUserRoles/{userId}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"GetUserRoles error: {response.StatusCode}, Content: {errorContent}");
                    return new List<string>();
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<string>>(json) ?? new List<string>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetUserRolesAsync: {ex.Message}");
                return new List<string>();
            }
        }

        public async Task<UpdateDetailsModel> GetUserByIdAsync(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());

            var response = await client.GetAsync($"{_apiBaseUrl}/api/AdminAPI/ViewEmployeeList/{id}");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API error: {response.StatusCode} - {error}");
            }

            var json = await response.Content.ReadAsStringAsync();

            // Đảm bảo ánh xạ đúng "role" vào "RoleName"
            var obj = JsonConvert.DeserializeObject<dynamic>(json);

            return new UpdateDetailsModel
            {
                Email = (string)obj.email,
                IsDisabled = (bool)obj.isDisabled,
                RoleName = (string)obj.role // ánh xạ từ "role" trong JSON
            };
        }
    }
}
