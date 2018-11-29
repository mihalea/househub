using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HouseHub.Data;
using HouseHub.Model;
using Microsoft.AspNetCore.Http;
using System.IO;
using HouseHub.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace HouseHub.Pages
{
    public class AddModel : BasePageModel
    {
        private readonly IHostingEnvironment _environment;

        [BindProperty] public FormData Input { get; set; }

        public class FormData
        {
            [Required]
            [Display(Name = "Image")]
            [ImageFile]
            public IFormFile ImageFile { get; set; }
            [Required]
            [MinLength(3)]
            [StringLength(50)]
            public string Name { get; set; }
            [Required]
            [Display(Name = "Short description (max 180 chars)")]
            [StringLength(180)]
            public string ShortDescription { get; set; }
            [Required]
            [StringLength(2000)]
            [DataType(DataType.MultilineText)]
            public string Description { get; set; }
        }

        public AddModel(
            IHostingEnvironment environment,
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager) : base(context, authorizationService, userManager)
        {
            _environment = environment;
        }

        public IActionResult OnGet()
        {
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var fileName = DateTime.UtcNow.ToString("yyyyMMdd-THHmmss-") + Input.ImageFile.FileName;
            var path = Path.Combine(_environment.ContentRootPath, "wwwroot/uploads", fileName);
            var shortPath = Path.Combine("/Uploads", fileName);
            var accommodation = new Accommodation
            {
                Name = Input.Name,
                ShortDescription = Input.ShortDescription,
                Description = Input.Description,
                ImagePath = shortPath,
                OwnerID = UserManager.GetUserAsync(User).Result.Id
            };

            var isAuthorised = await AuthorizationService.AuthorizeAsync(User, accommodation, Operations.Create);

            if (isAuthorised.Succeeded)
            {
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await Input.ImageFile.CopyToAsync(fileStream);
                }

                

                Context.Accommodation.Add(accommodation);
                Context.SaveChanges();
            }


            return RedirectToPage("./Index");
        }


    }
}