namespace SchoolAppCore.Data.Models;

public partial class Instructor
{
    public int InstructorId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateTime HireDate { get; set; }
}
