using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolAppCore.Data;
using SchoolAppCore.Data.Models;

namespace SchoolAppCoreRazor.Pages.Instructors
{
    public class DetailsModel : PageModel
    {
        private readonly SchoolAppCore.Data.SchoolContext _context;

        public DetailsModel(SchoolAppCore.Data.SchoolContext context)
        {
            _context = context;
        }

        public Instructor Instructor { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors.FirstOrDefaultAsync(m => m.InstructorId == id);
            if (instructor == null)
            {
                return NotFound();
            }
            else
            {
                Instructor = instructor;
            }
            return Page();
        }
    }
}
