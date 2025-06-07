create database HW1

-- Tạo bảng Students (Học sinh)
CREATE TABLE Students (
    StudentID INT PRIMARY KEY,
    Name NVARCHAR(100)
);

-- Tạo bảng Subjects (Môn học)
CREATE TABLE Subjects (
    SubjectID INT PRIMARY KEY,
    Name NVARCHAR(100)
);

-- Tạo bảng Grades (Đầu điểm)
CREATE TABLE Grades (
    GradeID INT PRIMARY KEY,
    StudentID INT,
    SubjectID INT,
    GradeType NVARCHAR(50),
    GradeValue DECIMAL(5, 2),  -- Giá trị điểm, ví dụ: 0.00 đến 10.00
    Weight DECIMAL(3, 2),      -- Trọng số, ví dụ: 0.20 cho 20%
    FOREIGN KEY (StudentID) REFERENCES Students(StudentID),
    FOREIGN KEY (SubjectID) REFERENCES Subjects(SubjectID)
);
CREATE PROCEDURE CalculateAverageGrade
    @StudentID int,
    @SubjectID int
AS
BEGIN
    SELECT SUM(GradeValue * Weight) AS Average
    FROM Grades
    WHERE StudentID = @StudentID AND SubjectID = @SubjectID
END

-- Thêm dữ liệu vào bảng Students
INSERT INTO Students (StudentID, Name)
VALUES 
    (1, 'Nguyen Van A'),
    (2, 'Tran Thi B'),
    (3, 'Le Van C');

-- Thêm dữ liệu vào bảng Subjects
INSERT INTO Subjects (SubjectID, Name)
VALUES 
    (1, 'PRN'),
    (2, 'PRO');

-- Thêm dữ liệu vào bảng Grades
-- Môn PRN (SubjectID = 1)
INSERT INTO Grades (GradeID, StudentID, SubjectID, GradeType, GradeValue, Weight)
VALUES 
    -- Học sinh 1
    (1, 1, 1, 'Lab', 7.5, 0.2),
    (2, 1, 1, 'Ass', 8.0, 0.2),
    (3, 1, 1, 'PE', 6.5, 0.3),
    (4, 1, 1, 'FE', 7.0, 0.3),
    -- Học sinh 2
    (5, 2, 1, 'Lab', 4.0, 0.2),
    (6, 2, 1, 'Ass', 5.5, 0.2),
    (7, 2, 1, 'PE', 6.0, 0.3),
    (8, 2, 1, 'FE', 5.0, 0.3),
    -- Học sinh 3
    (9, 3, 1, 'Lab', 2.0, 0.2),
    (10, 3, 1, 'Ass', 3.0, 0.2),
    (11, 3, 1, 'PE', 4.5, 0.3),
    (12, 3, 1, 'FE', 5.5, 0.3);

-- Môn PRO (SubjectID = 2)
INSERT INTO Grades (GradeID, StudentID, SubjectID, GradeType, GradeValue, Weight)
VALUES 
    -- Học sinh 1
    (13, 1, 2, 'Lab', 8.0, 0.3),
    (14, 1, 2, 'Ass', 7.5, 0.2),
    (15, 1, 2, 'PE', 8.5, 0.2),
    (16, 1, 2, 'FE', 9.0, 0.3),
    -- Học sinh 2
    (17, 2, 2, 'Lab', 6.0, 0.3),
    (18, 2, 2, 'Ass', 4.5, 0.2),
    (19, 2, 2, 'PE', 5.0, 0.2),
    (20, 2, 2, 'FE', 6.5, 0.3),
    -- Học sinh 3
    (21, 3, 2, 'Lab', 3.0, 0.3),
    (22, 3, 2, 'Ass', 4.0, 0.2),
    (23, 3, 2, 'PE', 2.5, 0.2),
    (24, 3, 2, 'FE', 5.0, 0.3);