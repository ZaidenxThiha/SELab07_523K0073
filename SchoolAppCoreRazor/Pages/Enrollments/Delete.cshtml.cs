using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolAppCore.Data;
using SchoolAppCore.Data.Models;

namespace SchoolAppCoreRazor.Pages.Enrollments
{
    public class DeleteModel : PageModel
    {
        private readonly SchoolAppCore.Data.SchoolContext _context;

        public DeleteModel(SchoolAppCore.Data.SchoolContext context)
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

            var enrollment = await _context.Enrollments.FirstOrDefaultAsync(m => m.EnrollmentId == id);

            if (enrollment == null)
            {
                return NotFound();
            }
            else
            {
                Enrollment = enrollment;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment != null)
            {
                Enrollment = enrollment;
                _context.Enrollments.Remove(Enrollment);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
