create databaSe DemoDTO



CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ProductName VARCHAR(MAX) NULL,
    Brand VARCHAR(MAX) NULL,
    Cost DECIMAL(18, 0) NOT NULL,
    ImageName VARCHAR(1024) NULL,
    Type VARCHAR(1238) NULL,
    CreateDate DATETIME NULL,
    ModifiedDate DATETIME NULL
);

INSERT INTO Products (ProductName, Brand, Cost, ImageName, Type, CreateDate, ModifiedDate)
VALUES 
('iPhone 14 Pro', 'Apple', 29990000, 'iphone14pro.png', 'Smartphone', '2024-01-01', '2024-05-01'),
('Galaxy S23 Ultra', 'Samsung', 27990000, 'galaxys23ultra.jpg', 'Smartphone', '2024-01-10', '2024-05-05'),
('ThinkPad X1 Carbon', 'Lenovo', 32990000, 'thinkpadx1.png', 'Laptop', '2023-12-15', '2024-04-20'),
('MacBook Pro M3', 'Apple', 59990000, 'macbookprom3.jpg', 'Laptop', '2024-02-01', '2024-05-10'),
('Sony WH-1000XM5', 'Sony', 7990000, 'sonyheadphone.jpg', 'Headphones', '2024-03-03', '2024-05-20');
