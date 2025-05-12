-- Tạo database (tuỳ chọn)
CREATE DATABASE ProductCategoryDB;
GO

USE ProductCategoryDB;
GO

-- Bảng Category
CREATE TABLE Category (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL
);

-- Bảng Product
CREATE TABLE Product (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Price DECIMAL(18, 2) NOT NULL
);

-- Bảng trung gian ProductCategory
CREATE TABLE ProductCategory (
    ProductId INT,
    CategoryId INT,
    PRIMARY KEY (ProductId, CategoryId),
    FOREIGN KEY (ProductId) REFERENCES Product(Id) ON DELETE CASCADE,
    FOREIGN KEY (CategoryId) REFERENCES Category(Id) ON DELETE CASCADE
);
