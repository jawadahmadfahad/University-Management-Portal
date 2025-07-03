USE UniversityDB;

SELECT * FROM Students;


CREATE TABLE Students (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100),
    Email NVARCHAR(100),
    Department NVARCHAR(100),
    CreatedAt DATETIME DEFAULT GETDATE()
);
CREATE TABLE Courses (
    Id INT PRIMARY KEY IDENTITY,
    Title NVARCHAR(100),
    Code NVARCHAR(50),
    CreditHours INT,
    CreatedAt DATETIME
);

CREATE TABLE Faculties (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100),
    Department NVARCHAR(100),
    Email NVARCHAR(100),
    CreatedAt DATETIME
);
