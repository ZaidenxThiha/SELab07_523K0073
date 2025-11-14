using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SchoolAppCore.Data.Models;

namespace SchoolAppCore.Data.Extensions;

public static class DatabaseExtensions
{
    public static async Task EnsureSeedDataAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<SchoolContext>();
        await context.Database.EnsureCreatedAsync();

        if (await context.Departments.AnyAsync())
        {
            return;
        }

        var departments = new List<Department>
        {
            new()
            {
                Name = "Computer Science",
                Budget = 120000m,
                StartDate = new DateTime(2023, 1, 1)
            },
            new()
            {
                Name = "Information Systems",
                Budget = 90000m,
                StartDate = new DateTime(2022, 6, 1)
            }
        };

        var instructors = new List<Instructor>
        {
            new()
            {
                FirstName = "John",
                LastName = "Doe",
                HireDate = new DateTime(2020, 8, 15)
            },
            new()
            {
                FirstName = "Anna",
                LastName = "Nguyen",
                HireDate = new DateTime(2021, 1, 10)
            }
        };

        var students = new List<Student>
        {
            new()
            {
                FirstName = "Jane",
                LastName = "Smith",
                EnrollmentDate = new DateTime(2023, 9, 1)
            },
            new()
            {
                FirstName = "David",
                LastName = "Tran",
                EnrollmentDate = new DateTime(2023, 9, 1)
            }
        };

        context.Departments.AddRange(departments);
        context.Instructors.AddRange(instructors);
        context.Students.AddRange(students);

        var courses = new List<Course>
        {
            new()
            {
                Title = "Introduction to Programming",
                Credits = 3,
                Department = departments[0]
            },
            new()
            {
                Title = "Data Management",
                Credits = 3,
                Department = departments[1]
            }
        };

        context.Courses.AddRange(courses);

        var enrollments = new List<Enrollment>
        {
            new()
            {
                Course = courses[0],
                Student = students[0],
                Grade = 3.5m
            },
            new()
            {
                Course = courses[1],
                Student = students[1],
                Grade = 3.8m
            }
        };

        context.Enrollments.AddRange(enrollments);

        await context.SaveChangesAsync();
    }
}
