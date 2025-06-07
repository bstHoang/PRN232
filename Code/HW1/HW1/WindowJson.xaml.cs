using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HW1.Models;
using HW1.Services;

namespace HW1
{
    /// <summary>
    /// Interaction logic for WindowJson.xaml
    /// </summary>
    public partial class WindowJson : Window
    {
        private readonly GradeServiceJson _service;
        private readonly Hw1Context _context;

        public WindowJson()
        {
            InitializeComponent();
            _context = new Hw1Context(); // DbContext từ scaffold
            _service = new GradeServiceJson(_context); // GradeService đã cập nhật với JSON
            LoadStudents();
            LoadSubjects();
        }
        private void LoadStudents()
        {
            var students = _context.Students.ToList();
            StudentList.ItemsSource = students;
        }
        private void LoadSubjects()
        {
            var subjects = _context.Subjects.ToList();
            SubjectComboBox.ItemsSource = subjects;
            if (subjects.Any())
                SubjectComboBox.SelectedIndex = 0; // Chọn môn đầu tiên mặc định
        }
        private void StudentList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            UpdateAverage();
        }

        private void SubjectComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            UpdateAverage();
        }

        private void UpdateAverage()
        {
            var selectedStudent = StudentList.SelectedItem as Student;
            var selectedSubject = SubjectComboBox.SelectedItem as Subject;
            if (selectedStudent != null && selectedSubject != null)
            {
                var average = _service.CalculateAverageGrade(selectedStudent.StudentId, selectedSubject.SubjectId);
                AverageText.Text = average.ToString("F2");
                ResultText.Text = string.Empty; // Xóa kết quả cũ
            }
            else
            {
                AverageText.Text = string.Empty;
                ResultText.Text = string.Empty;
            }
        }

        private void CheckCase1_Click(object sender, RoutedEventArgs e)
        {
            var selectedStudent = StudentList.SelectedItem as Student;
            var selectedSubject = SubjectComboBox.SelectedItem as Subject;
            if (selectedStudent != null && selectedSubject != null)
            {
                bool isPass = _service.IsPassCase1(selectedStudent.StudentId, selectedSubject.SubjectId);
                ResultText.Text = isPass ? "Pass" : "Fail";
            }
            else
            {
                ResultText.Text = "Vui lòng chọn học sinh và môn học";
            }
        }

        private void CheckCase2_Click(object sender, RoutedEventArgs e)
        {
            var selectedStudent = StudentList.SelectedItem as Student;
            var selectedSubject = SubjectComboBox.SelectedItem as Subject;
            if (selectedStudent != null && selectedSubject != null)
            {
                bool isPass = _service.IsPassCase2(selectedStudent.StudentId, selectedSubject.SubjectId);
                ResultText.Text = isPass ? "Pass" : "Fail";
            }
            else
            {
                ResultText.Text = "Vui lòng chọn học sinh và môn học";
            }
        }
    }
}