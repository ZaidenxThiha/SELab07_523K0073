namespace SchoolAppCore.Data.Models;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Budget { get; set; }

    public DateTime StartDate { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
