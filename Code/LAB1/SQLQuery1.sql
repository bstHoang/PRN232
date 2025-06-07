CREATE DATABASE NewsWebsiteDb;
GO

USE NewsWebsiteDb;
GO

CREATE TABLE Categories (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE News (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CategoryId INT NOT NULL,
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
);
GO

-- Thêm d? li?u m?u
INSERT INTO Categories (Name) VALUES (N'Th? thao'), (N'Gi?i trí'), (N'Chính tr?');
INSERT INTO News (Title, Content, CategoryId) VALUES
(N'Tin th? thao 1', N'N?i dung tin th? thao...', 1),
(N'Tin gi?i trí 1', N'N?i dung tin gi?i trí...', 2);
