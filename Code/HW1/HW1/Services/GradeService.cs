using HW1.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HW1.Services
{
    internal class GradeService
    {
        private readonly Hw1Context _context;

        public GradeService(Hw1Context context)
        {
            _context = context;
        }

        // Tính điểm trung bình
        public decimal CalculateAverageGrade(int studentID, int subjectID)
        {
            var grades = _context.Grades
                .Where(g => g.StudentId == studentID && g.SubjectId == subjectID)
                .ToList();

            if (grades.Count == 0) return 0;

            return (decimal)grades.Sum(g => g.GradeValue * g.Weight);
        }

        // Trường hợp 1: Tất cả đầu điểm > 0 và trung bình >= 5
        public bool IsPassCase1(int studentID, int subjectID)
        {
            var grades = _context.Grades
                .Where(g => g.StudentId == studentID && g.SubjectId == subjectID)
                .ToList();

            if (grades.Any(g => g.GradeValue <= 0)) return false;

            var average = CalculateAverageGrade(studentID, subjectID);
            return average >= 5;
        }

        // Trường hợp 2: Trung bình >= 5, tất cả đầu điểm > 0, đầu điểm 20% >= 4
        public bool IsPassCase2(int studentID, int subjectID)
        {
            var grades = _context.Grades
                .Where(g => g.StudentId == studentID && g.SubjectId == subjectID)
                .ToList();

            if (grades.Any(g => g.GradeValue <= 0)) return false;
            if (grades.Any(g => g.Weight == 0.2m && g.GradeValue < 4)) return false;

            var average = CalculateAverageGrade(studentID, subjectID);
            return average >= 5;
        }

        public decimal CalculateAverageGradeFromSql(int studentID, int subjectID)
        {
            var studentIdParam = new SqlParameter("@StudentID", studentID);
            var subjectIdParam = new SqlParameter("@SubjectID", subjectID);

            var result = _context.Database
                .SqlQueryRaw<decimal>("EXEC CalculateAverageGrade @StudentID, @SubjectID", studentIdParam, subjectIdParam)
                .SingleOrDefault();

            return result;
        }
    }
}
