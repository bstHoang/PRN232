-- T?o b?ng Roles (Vai trò)
CREATE TABLE Roles (
      RoleID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100)
);

-- T?o b?ng Users (Ng??i dùng, thay th? b?ng Students)
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Account NVARCHAR(100) NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    RoleID INT,
    FOREIGN KEY (RoleID) REFERENCES Roles(RoleID)
);

-- T?o b?ng Subjects (Môn h?c) – gi? nguyên
CREATE TABLE Subjects (
     SubjectID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100)
);

-- T?o b?ng Grades (?i?m)
CREATE TABLE Grades (
    GradeID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT,                      -- c?p nh?t t? StudentID thành UserID
    SubjectID INT,
    GradeType NVARCHAR(50),
    GradeValue DECIMAL(5, 2),        -- Giá tr? ?i?m, ví d?: 0.00 ??n 10.00
    Weight DECIMAL(3, 2),            -- Tr?ng s?, ví d?: 0.20 cho 20%
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (SubjectID) REFERENCES Subjects(SubjectID)
);
INSERT INTO Roles (Name) VALUES
(N'Admin'),
(N'Teacher'),
(N'Students');
INSERT INTO Users (Account, Password, RoleID) VALUES
('student1', 'adminpass', 3),
('student2', 'teachpass', 3),
('student3', 'studpass1', 3),
('Admin', '123', 1),
('Teacher', '123', 2);



INSERT INTO Subjects (Name) VALUES
('PRN'),
('PRO');

INSERT INTO Grades (UserID, SubjectID, GradeType, GradeValue, Weight)
VALUES 
    -- student1 - PRN
    (1, 1, 'Lab', 7.5, 0.2),
    (1, 1, 'Ass', 8.0, 0.2),
    (1, 1, 'PE', 6.5, 0.3),
    (1, 1, 'FE', 7.0, 0.3),
    -- student2 - PRN
    (2, 1, 'Lab', 4.0, 0.2),
    (2, 1, 'Ass', 5.5, 0.2),
    (2, 1, 'PE', 6.0, 0.3),
    (2, 1, 'FE', 5.0, 0.3),
    -- student3 - PRN
    (3, 1, 'Lab', 2.0, 0.2),
    (3, 1, 'Ass', 3.0, 0.2),
    (3, 1, 'PE', 4.5, 0.3),
    (3, 1, 'FE', 5.5, 0.3),
    -- student1 - PRO
    (1, 2, 'Lab', 8.0, 0.3),
    (1, 2, 'Ass', 7.5, 0.2),
    (1, 2, 'PE', 8.5, 0.2),
    (1, 2, 'FE', 9.0, 0.3),
    -- student2 - PRO
    (2, 2, 'Lab', 6.0, 0.3),
    (2, 2, 'Ass', 4.5, 0.2),
    (2, 2, 'PE', 5.0, 0.2),
    (2, 2, 'FE', 6.5, 0.3),
    -- student3 - PRO
    (3, 2, 'Lab', 3.0, 0.3),
    (3, 2, 'Ass', 4.0, 0.2),
    (3, 2, 'PE', 2.5, 0.2),
    (3, 2, 'FE', 5.0, 0.3);