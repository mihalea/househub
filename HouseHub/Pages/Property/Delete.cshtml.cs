using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseHub.Data;
using HouseHub.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HouseHub.Pages.Property
{
    public class DeleteModel : BasePageModel
    {
        [BindProperty] public Accommodation Accommodation { get; set; }

        public DeleteModel(
            ApplicationDbContext context, 
            IAuthorizationService authorizationService, 
            UserManager<ApplicationUser> userManager) : base(context, authorizationService, userManager)
        {
        }

        public async Task OnGetAsync(int id)
        {
            Accommodation = await Context.Accommodation.FindAsync(id);
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Context.Accommodation.Remove(new Accommodation
            {
                AccommodationID = id
            });
            await Context.SaveChangesAsync();

            TempData["Message"] = "Successfully deleted property";

            return RedirectToPage("/Property/Index");
        }
    }
}