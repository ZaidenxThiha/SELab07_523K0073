using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolAppCore.Data.Models;

namespace SchoolAppCoreRazor.Pages.Enrollments
{
    public class CreateModel : PageModel
    {
        private readonly SchoolAppCore.Data.SchoolContext _context;

        public CreateModel(SchoolAppCore.Data.SchoolContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadSelectListsAsync();
            return Page();
        }

        [BindProperty]
        public Enrollment Enrollment { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadSelectListsAsync();
                return Page();
            }

            _context.Enrollments.Add(Enrollment);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private async Task LoadSelectListsAsync()
        {
            var courses = await _context.Courses
                .OrderBy(c => c.Title)
                .Select(c => new { c.CourseId, c.Title })
                .ToListAsync();
            ViewData["CourseId"] = new SelectList(courses, "CourseId", "Title");

            var students = await _context.Students
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .Select(s => new
                {
                    s.StudentId,
                    FullName = s.FirstName + " " + s.LastName
                })
                .ToListAsync();
            ViewData["StudentId"] = new SelectList(students, "StudentId", "FullName");
        }
    }
}
