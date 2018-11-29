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
using Microsoft.EntityFrameworkCore;

namespace HouseHub.Pages.Property
{
    public class PendingModel : BasePageModel
    {
        [BindProperty] public IList<Accommodation> Pending { get; set; }
        [BindProperty] public IList<Accommodation> Rejected { get; set; }
        [BindProperty] public IList<Accommodation> Approved { get; set; }


        public PendingModel(
            ApplicationDbContext context, 
            IAuthorizationService authorizationService, 
            UserManager<ApplicationUser> userManager) : base(context, authorizationService, userManager)
        {
        }

        public async Task OnGetAsync()
        {
            var user = await UserManager.GetUserAsync(User);
            var owned = Context.Accommodation.Where(a => a.OwnerID == user.Id);
            Pending = await owned
                .Where(a => a.Pending == true)
                .ToListAsync();
            Rejected = await owned
                .Where(a => a.Pending == false && a.Approved == false)
                .ToListAsync();
            Approved = await owned
                .Where(a => a.Pending == false && a.Approved == true)
                .ToListAsync();

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