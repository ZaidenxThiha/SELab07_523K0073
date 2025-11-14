using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolAppCore.Data.Models;

namespace SchoolAppCoreRazor.Pages.Enrollments
{
    public class EditModel : PageModel
    {
        private readonly SchoolAppCore.Data.SchoolContext _context;

        public EditModel(SchoolAppCore.Data.SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Enrollment Enrollment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment =  await _context.Enrollments.FirstOrDefaultAsync(m => m.EnrollmentId == id);
            if (enrollment == null)
            {
                return NotFound();
            }
            Enrollment = enrollment;
            await LoadSelectListsAsync(Enrollment.CourseId, Enrollment.StudentId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadSelectListsAsync(Enrollment.CourseId, Enrollment.StudentId);
                return Page();
            }

            _context.Attach(Enrollment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnrollmentExists(Enrollment.EnrollmentId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool EnrollmentExists(int id)
        {
            return _context.Enrollments.Any(e => e.EnrollmentId == id);
        }

        private async Task LoadSelectListsAsync(int? selectedCourseId = null, int? selectedStudentId = null)
        {
            var courses = await _context.Courses
                .OrderBy(c => c.Title)
                .Select(c => new { c.CourseId, c.Title })
                .ToListAsync();
            ViewData["CourseId"] = new SelectList(courses, "CourseId", "Title", selectedCourseId);

            var students = await _context.Students
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .Select(s => new
                {
                    s.StudentId,
                    FullName = s.FirstName + " " + s.LastName
                })
                .ToListAsync();
            ViewData["StudentId"] = new SelectList(students, "StudentId", "FullName", selectedStudentId);
        }
    }
}
