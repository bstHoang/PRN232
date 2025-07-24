
-- Tạo cơ sở dữ liệu
CREATE DATABASE EcommerceDB;
GO

-- Sử dụng cơ sở dữ liệu
USE EcommerceDB;
GO


-- Tạo bảng
CREATE TABLE Customers (
    CustomerID INT IDENTITY PRIMARY KEY,
    FullName NVARCHAR(100),
    Email NVARCHAR(100)
);

CREATE TABLE Products (
    ProductID INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(100),
    Price DECIMAL(10,2)
);

CREATE TABLE Tags (
    TagID INT IDENTITY PRIMARY KEY,
    TagName NVARCHAR(50)
);

CREATE TABLE Orders (
    OrderID INT IDENTITY PRIMARY KEY,
    CustomerID INT FOREIGN KEY REFERENCES Customers(CustomerID),
    OrderDate DATE
);

CREATE TABLE OrderProduct (
    OrderID INT,
    ProductID INT,
    Quantity INT,
    PRIMARY KEY (OrderID, ProductID),
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

CREATE TABLE ProductTag (
    ProductID INT,
    TagID INT,
    PRIMARY KEY (ProductID, TagID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    FOREIGN KEY (TagID) REFERENCES Tags(TagID)
);

-- Dữ liệu mẫu
INSERT INTO Customers (FullName, Email) VALUES (N'Customer 1', N'customer1@example.com');
INSERT INTO Customers (FullName, Email) VALUES (N'Customer 2', N'customer2@example.com');
INSERT INTO Customers (FullName, Email) VALUES (N'Customer 3', N'customer3@example.com');
INSERT INTO Customers (FullName, Email) VALUES (N'Customer 4', N'customer4@example.com');
INSERT INTO Customers (FullName, Email) VALUES (N'Customer 5', N'customer5@example.com');

INSERT INTO Products (Name, Price) VALUES (N'Product 1', 183.11);
INSERT INTO Products (Name, Price) VALUES (N'Product 2', 354.85);
INSERT INTO Products (Name, Price) VALUES (N'Product 3', 302.65);
INSERT INTO Products (Name, Price) VALUES (N'Product 4', 319.62);
INSERT INTO Products (Name, Price) VALUES (N'Product 5', 22.6);

INSERT INTO Tags (TagName) VALUES (N'Electronics');
INSERT INTO Tags (TagName) VALUES (N'Home');
INSERT INTO Tags (TagName) VALUES (N'Fashion');
INSERT INTO Tags (TagName) VALUES (N'Books');
INSERT INTO Tags (TagName) VALUES (N'Toys');

INSERT INTO Orders (CustomerID, OrderDate) VALUES (1, N'2025-06-11');
INSERT INTO Orders (CustomerID, OrderDate) VALUES (2, N'2025-06-12');
INSERT INTO Orders (CustomerID, OrderDate) VALUES (3, N'2025-06-13');
INSERT INTO Orders (CustomerID, OrderDate) VALUES (4, N'2025-06-14');
INSERT INTO Orders (CustomerID, OrderDate) VALUES (5, N'2025-06-15');

INSERT INTO OrderProduct (OrderID, ProductID, Quantity) VALUES (1, 1, 4);
INSERT INTO OrderProduct (OrderID, ProductID, Quantity) VALUES (1, 2, 5);
INSERT INTO OrderProduct (OrderID, ProductID, Quantity) VALUES (2, 1, 3);
INSERT INTO OrderProduct (OrderID, ProductID, Quantity) VALUES (2, 2, 1);
INSERT INTO OrderProduct (OrderID, ProductID, Quantity) VALUES (3, 1, 3);
INSERT INTO OrderProduct (OrderID, ProductID, Quantity) VALUES (3, 2, 2);
INSERT INTO OrderProduct (OrderID, ProductID, Quantity) VALUES (4, 1, 1);
INSERT INTO OrderProduct (OrderID, ProductID, Quantity) VALUES (4, 2, 3);
INSERT INTO OrderProduct (OrderID, ProductID, Quantity) VALUES (5, 1, 4);
INSERT INTO OrderProduct (OrderID, ProductID, Quantity) VALUES (5, 2, 3);

INSERT INTO ProductTag (ProductID, TagID) VALUES (1, 2);
INSERT INTO ProductTag (ProductID, TagID) VALUES (2, 3);
INSERT INTO ProductTag (ProductID, TagID) VALUES (3, 4);
INSERT INTO ProductTag (ProductID, TagID) VALUES (4, 5);
INSERT INTO ProductTag (ProductID, TagID) VALUES (5, 1);
INSERT INTO ProductTag (ProductID, TagID) VALUES (1, 3);
INSERT INTO ProductTag (ProductID, TagID) VALUES (2, 4);
INSERT INTO ProductTag (ProductID, TagID) VALUES (3, 5);
INSERT INTO ProductTag (ProductID, TagID) VALUES (4, 1);
INSERT INTO ProductTag (ProductID, TagID) VALUES (5, 2);
