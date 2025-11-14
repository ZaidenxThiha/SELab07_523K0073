using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolAppCore.Data.Models;

namespace SchoolAppCoreRazor.Pages.Courses
{
    public class EditModel : PageModel
    {
        private readonly SchoolAppCore.Data.SchoolContext _context;

        public EditModel(SchoolAppCore.Data.SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Course Course { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course =  await _context.Courses.FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }
            Course = course;
            await LoadDepartmentsAsync(Course.DepartmentId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadDepartmentsAsync(Course.DepartmentId);
                return Page();
            }

            _context.Attach(Course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(Course.CourseId))
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

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }

        private async Task LoadDepartmentsAsync(int? selectedDepartmentId = null)
        {
            var departments = await _context.Departments
                .OrderBy(d => d.Name)
                .Select(d => new { d.DepartmentId, d.Name })
                .ToListAsync();

            ViewData["DepartmentId"] = new SelectList(departments, "DepartmentId", "Name", selectedDepartmentId);
        }
    }
}
