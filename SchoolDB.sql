-- SchoolDB schema and seed data used by both Razor Pages and MVC apps
IF DB_ID('SchoolDB') IS NULL
BEGIN
    CREATE DATABASE SchoolDB;
END
GO

USE SchoolDB;
GO

IF OBJECT_ID('dbo.Enrollments', 'U') IS NOT NULL DROP TABLE dbo.Enrollments;
IF OBJECT_ID('dbo.Courses', 'U') IS NOT NULL DROP TABLE dbo.Courses;
IF OBJECT_ID('dbo.Students', 'U') IS NOT NULL DROP TABLE dbo.Students;
IF OBJECT_ID('dbo.Instructors', 'U') IS NOT NULL DROP TABLE dbo.Instructors;
IF OBJECT_ID('dbo.Departments', 'U') IS NOT NULL DROP TABLE dbo.Departments;
GO

CREATE TABLE Departments (
    DepartmentID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    Budget DECIMAL(18,2) NOT NULL,
    StartDate DATE NOT NULL
);

CREATE TABLE Instructors (
    InstructorID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    HireDate DATE NOT NULL
);

CREATE TABLE Courses (
    CourseID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Title NVARCHAR(100) NOT NULL,
    Credits INT NOT NULL,
    DepartmentID INT NOT NULL,
    CONSTRAINT FK_Courses_Departments FOREIGN KEY (DepartmentID)
        REFERENCES Departments(DepartmentID)
);

CREATE TABLE Students (
    StudentID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    EnrollmentDate DATE NOT NULL
);

CREATE TABLE Enrollments (
    EnrollmentID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CourseID INT NOT NULL,
    StudentID INT NOT NULL,
    Grade DECIMAL(3,2) NULL,
    CONSTRAINT FK_Enrollments_Courses FOREIGN KEY (CourseID)
        REFERENCES Courses(CourseID),
    CONSTRAINT FK_Enrollments_Students FOREIGN KEY (StudentID)
        REFERENCES Students(StudentID)
);

-- seed data
INSERT INTO Departments (Name, Budget, StartDate)
VALUES ('Computer Science', 120000.00, '2023-01-01'),
       ('Information Systems', 90000.00, '2022-06-01');

INSERT INTO Instructors (FirstName, LastName, HireDate)
VALUES ('John', 'Doe', '2020-08-15'),
       ('Anna', 'Nguyen', '2021-01-10');

INSERT INTO Courses (Title, Credits, DepartmentID)
VALUES ('Introduction to Programming', 3, 1),
       ('Data Management', 3, 2);

INSERT INTO Students (FirstName, LastName, EnrollmentDate)
VALUES ('Jane', 'Smith', '2023-09-01'),
       ('David', 'Tran', '2023-09-01');

INSERT INTO Enrollments (CourseID, StudentID, Grade)
VALUES (1, 1, 3.50),
       (2, 2, 3.80);
GO
