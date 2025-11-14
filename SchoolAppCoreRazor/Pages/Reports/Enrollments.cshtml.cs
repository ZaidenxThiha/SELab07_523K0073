using AspNetCore.Reporting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolAppCore.Data;
using System.IO;
using System.Text;

namespace SchoolAppCoreRazor.Pages.Reports;

public class EnrollmentsModel : PageModel
{
    private readonly SchoolContext _context;
    private readonly IWebHostEnvironment _environment;

    public EnrollmentsModel(SchoolContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var rows = await _context.Enrollments
            .Include(e => e.Course)
            .Include(e => e.Student)
            .OrderBy(e => e.Course != null ? e.Course.Title : string.Empty)
            .ThenBy(e => e.Student != null ? e.Student.LastName : string.Empty)
            .Select(e => new EnrollmentRow
            {
                CourseTitle = e.Course != null ? e.Course.Title : string.Empty,
                StudentFullName = e.Student != null ? e.Student.FirstName + " " + e.Student.LastName : string.Empty,
                Grade = e.Grade ?? 0m
            })
            .ToListAsync();

        var reportPath = ResolveReportPath();

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        var localReport = new LocalReport(reportPath);
        localReport.AddDataSource("EnrollmentDataSet", rows);
        var result = localReport.Execute(RenderType.Pdf, 1, parameters: null);

        return File(result.MainStream, "application/pdf", "EnrollmentsReport.pdf");
    }

    private class EnrollmentRow
    {
        public string CourseTitle { get; set; } = string.Empty;
        public string StudentFullName { get; set; } = string.Empty;
        public decimal Grade { get; set; }
    }

    private string ResolveReportPath()
    {
        var candidates = new[]
        {
            Path.Combine(_environment.ContentRootPath, "Reports", "EnrollmentsReport.rdlc"),
            Path.Combine(_environment.ContentRootPath, "..", "Reports", "EnrollmentsReport.rdlc"),
            Path.Combine(AppContext.BaseDirectory, "Reports", "EnrollmentsReport.rdlc"),
        }
        .Select(Path.GetFullPath);

        foreach (var candidate in candidates)
        {
            if (System.IO.File.Exists(candidate))
            {
                return candidate;
            }
        }

        throw new FileNotFoundException("Unable to locate EnrollmentsReport.rdlc. Checked: " +
            string.Join(Environment.NewLine, candidates));
    }
}
