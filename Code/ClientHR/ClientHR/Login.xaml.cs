using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text;
using System.Windows;

using Newtonsoft.Json;

namespace ClientHR
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string ApiUrl = "http://localhost:5265/api";
        public Login()
        {
            InitializeComponent();
        }
        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            var username = txtUsername.Text;
            var password = txtPassword.Password;

            var loginData = new { userName = username, password = password };
            var content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync($"{ApiUrl}/AccountAPI/Login", content);
                response.EnsureSuccessStatusCode();

                var responseData = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
                string token = responseData.token;

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                string department = jwtToken.Claims.FirstOrDefault(c => c.Type == "Department")?.Value;

                if (department != "HR")
                {
                    MessageBox.Show("Chỉ thành viên phòng HR được phép đăng nhập.", "Từ chối truy cập",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Lưu token vào App
                App.Token = token;

                // Mở cửa sổ chính
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đăng nhập thất bại: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
