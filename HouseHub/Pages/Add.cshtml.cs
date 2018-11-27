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
using Microsoft.AspNetCore.Identity;

namespace HouseHub.Pages
{
    public class AddModel : BasePageModel
    {
        private readonly IHostingEnvironment _environment;

        [BindProperty] public IFormFile ImageFile { get; set; }
        [BindProperty] public string Name { get; set; }
        [BindProperty] public string Description { get; set; }

        public AddModel(
            IHostingEnvironment environment,
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager) : base(context, authorizationService, userManager)
        {
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
            var shortPath = Path.Combine("Uploads", fileName);
            var accommodation = new Accommodation
            {
                Name = Name,
                Description = Description,
                ImagePath = shortPath
            };

            var isAuthorised = await AuthorizationService.AuthorizeAsync(User, accommodation, Operations.Create);

            if (isAuthorised.Succeeded)
            {
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }

                

                Context.Accommodation.Add(accommodation);
                Context.SaveChanges();
            }


            return RedirectToPage("./Index");
        }


    }
}