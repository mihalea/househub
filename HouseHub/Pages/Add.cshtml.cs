using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HouseHub.Data;
using HouseHub.Model;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace HouseHub.Pages
{
    [Authorize(Roles = "Admin")]
    public class AddModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;

        [BindProperty] public IFormFile ImageFile { get; set; }
        [BindProperty] public string Name { get; set; }
        [BindProperty] public string Description { get; set; }

        public AddModel(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string Name, string Description)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var fileName = DateTime.UtcNow.ToString("yyyyMMdd-THHmmss-") + ImageFile.FileName;
            var path = Path.Combine(_environment.ContentRootPath, "Uploads", fileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await ImageFile.CopyToAsync(fileStream);
            }

            var accommodation = new Accommodation {
                Name = Name,
                Description = Description,
                ImagePath = path
            };

            _context.Accommodation.Add(accommodation);
            _context.SaveChanges();
         

            return RedirectToPage("./Index");
        }
    }
}