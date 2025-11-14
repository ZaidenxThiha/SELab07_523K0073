using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolAppCore.Data.Models;

namespace SchoolAppCoreRazor.Pages.Courses
{
    public class CreateModel : PageModel
    {
        private readonly SchoolAppCore.Data.SchoolContext _context;

        public CreateModel(SchoolAppCore.Data.SchoolContext context)
        {
            _context = context;
        }

        public SelectList DepartmentOptions { get; set; } = default!;

        [BindProperty]
        public Course Course { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadDepartmentsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadDepartmentsAsync();
                return Page();
            }

            _context.Courses.Add(Course);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private async Task LoadDepartmentsAsync()
        {
            var departments = await _context.Departments
                .OrderBy(d => d.Name)
                .Select(d => new { d.DepartmentId, d.Name })
                .ToListAsync();

            ViewData["DepartmentId"] = new SelectList(departments, "DepartmentId", "Name");
        }
    }
}
