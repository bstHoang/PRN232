
-- Roles Table
CREATE TABLE Roles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL UNIQUE
);

-- Accounts Table (sửa để tương thích với custom Identity)
CREATE TABLE Accounts (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Password NVARCHAR(100) NOT NULL,
    RoleId INT NOT NULL,
    FOREIGN KEY (RoleId) REFERENCES Roles(Id)
);

-- Categories Table
CREATE TABLE Categories (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL
);

-- News Table
CREATE TABLE News (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(500),
    Content NVARCHAR(MAX),
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CategoryId INT NOT NULL,
    CreateBy INT NOT NULL,
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
    FOREIGN KEY (CreateBy) REFERENCES Accounts(Id)
);

-- Tags Table
CREATE TABLE Tags (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL
);

-- NewsTags Table (Many-to-Many)
CREATE TABLE NewsTags (
    Id_Tags INT NOT NULL,
    Id_News INT NOT NULL,
    PRIMARY KEY (Id_Tags, Id_News),
    FOREIGN KEY (Id_Tags) REFERENCES Tags(Id),
    FOREIGN KEY (Id_News) REFERENCES News(Id)
);