using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ClientHR.Models;
using Newtonsoft.Json;

namespace ClientHR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string ApiUrl = "http://localhost:5265/api";
        private List<Employee> employees;
        private List<Department> departments;
        private List<Position> positions;

        public MainWindow()
        {
            InitializeComponent();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.Token);
            LoadDepartmentsAndPositions();
            LoadEmployees();
        }
        private async void LoadDepartmentsAndPositions()
        {
            try
            {
                // Tải phòng ban
                var deptResponse = await _httpClient.GetAsync($"{ApiUrl}/DepartmentsAPI");
                deptResponse.EnsureSuccessStatusCode();
                departments = JsonConvert.DeserializeObject<List<Department>>(await deptResponse.Content.ReadAsStringAsync());
                cmbDepartment.ItemsSource = departments;
                icDepartments.ItemsSource = departments;

                // Tải vị trí
                var posResponse = await _httpClient.GetAsync($"{ApiUrl}/PositionsAPI");
                posResponse.EnsureSuccessStatusCode();
                positions = JsonConvert.DeserializeObject<List<Position>>(await posResponse.Content.ReadAsStringAsync());
                cmbPosition.ItemsSource = positions;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Tải danh sách nhân viên
        private async void LoadEmployees(string filter = null)
        {
            try
            {
                string url = $"{ApiUrl}/HRAPI/ViewEmployeeList";
                if (!string.IsNullOrEmpty(filter))
                {
                    url += $"?$filter={filter}"; // Hỗ trợ OData
                }
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                employees = JsonConvert.DeserializeObject<List<Employee>>(json);
                dgEmployees.ItemsSource = employees;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách nhân viên: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton rb && rb.Tag != null)
            {
                string filter = null;
                if (rb.Tag.ToString() != "All")
                {
                    int departmentId;
                    if (int.TryParse(rb.Tag.ToString(), out departmentId))
                    {
                        var department = departments?.FirstOrDefault(d => d.Id == departmentId);
                        if (department != null)
                        {
                            filter = $"Department eq '{department.Name}'";
                        }
                    }
                }
                LoadEmployees(filter);
            }
        }
        private void dgEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgEmployees.SelectedItem is Employee selectedEmployee)
            {
                txtId.Text = selectedEmployee.Id.ToString();
                txtUserName.Text = selectedEmployee.UserName;
                txtName.Text = selectedEmployee.Name;
                cmbDepartment.SelectedValue = departments.FirstOrDefault(d => d.Name == selectedEmployee.Department)?.Id;
                cmbPosition.SelectedValue = positions.FirstOrDefault(p => p.Name == selectedEmployee.Position)?.Id;
            }
        }

        private async void UpdateEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtId.Text, out int id))
            {
                var updatedEmployee = new CreateEmployeeModel
                {
                    UserName = txtUserName.Text,
                    Name = txtName.Text,
                    DepartmentId = (int)cmbDepartment.SelectedValue,
                    PositionId = (int)cmbPosition.SelectedValue
                };

                var content = new StringContent(JsonConvert.SerializeObject(updatedEmployee), Encoding.UTF8, "application/json");
                try
                {
                    var response = await _httpClient.PutAsync($"{ApiUrl}/HRAPI/Employees/{id}", content);
                    response.EnsureSuccessStatusCode();
                    LoadEmployees();
                    MessageBox.Show("Cập nhật nhân viên thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi cập nhật nhân viên: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void CreateEmployee_Click(object sender, RoutedEventArgs e)
        {
            var newEmployee = new CreateEmployeeModel
            {
                UserName = txtUserName.Text,
                Name = txtName.Text,
                DepartmentId = (int)cmbDepartment.SelectedValue,
                PositionId = (int)cmbPosition.SelectedValue
            };

            var content = new StringContent(JsonConvert.SerializeObject(newEmployee), Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync($"{ApiUrl}/HRAPI/Employees", content);
                response.EnsureSuccessStatusCode();
                LoadEmployees();
                MessageBox.Show("Tạo nhân viên mới thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo nhân viên: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtId.Text, out int id))
            {
                try
                {
                    var response = await _httpClient.DeleteAsync($"{ApiUrl}/HRAPI/Employees/{id}");
                    response.EnsureSuccessStatusCode();
                    LoadEmployees();
                    MessageBox.Show("Xóa nhân viên thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa nhân viên: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}