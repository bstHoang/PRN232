
using System.IO;
using HW1.Config;
using HW1.Models;
using Newtonsoft.Json;

namespace HW1.Services
{
    internal class GradeServiceJson
    {
        private readonly Hw1Context _context;
        private readonly PassConfig _config;

        public GradeServiceJson(Hw1Context context)
        {
            _context = context;
            _config = LoadConfig(); // Đọc cấu hình từ JSON khi khởi tạo
        }

        private PassConfig LoadConfig()
        {
            var json = File.ReadAllText("config.json"); // Đọc file JSON
            return JsonConvert.DeserializeObject<PassConfig>(json); // Chuyển JSON thành object
        }

        public decimal CalculateAverageGrade(int studentID, int subjectID)
        {
            var grades = _context.Grades
                .Where(g => g.StudentId == studentID && g.SubjectId == subjectID)
                .ToList();

            if (grades.Count == 0) return 0;

            return (decimal)grades.Sum(g => g.GradeValue * g.Weight);
        }

        public bool IsPassCase1(int studentID, int subjectID)
        {
            var grades = _context.Grades
                .Where(g => g.StudentId == studentID && g.SubjectId == subjectID)
                .ToList();

            if (grades.Any(g => g.GradeValue <= _config.PassConditions.Case1.AllGradesAbove))
                return false;

            var average = CalculateAverageGrade(studentID, subjectID);
            return average >= _config.PassConditions.Case1.AverageAbove;
        }

        public bool IsPassCase2(int studentID, int subjectID)
        {
            var grades = _context.Grades
                .Where(g => g.StudentId == studentID && g.SubjectId == subjectID)
                .ToList();

            if (grades.Any(g => g.GradeValue <= _config.PassConditions.Case2.AllGradesAbove))
                return false;
            if (grades.Any(g => g.Weight == 0.2m && g.GradeValue < _config.PassConditions.Case2.Weight20PercentMin))
                return false;

            var average = CalculateAverageGrade(studentID, subjectID);
            return average >= _config.PassConditions.Case2.AverageAbove;
        }
    }
}