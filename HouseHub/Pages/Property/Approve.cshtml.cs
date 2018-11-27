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
    public class ApproveModel : BasePageModel
    {
        [BindProperty] public IList<Accommodation> AccommodationList { get; set; }
        [BindProperty] public Accommodation Accommodation { get; set; }

        public ApproveModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager) : base(context, authorizationService, userManager)
        {
        }

        
        public async Task OnGetAsync()
        {
            AccommodationList = await Context.Accommodation.Where(a => a.Approved == false).ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Accommodation = Context.Accommodation.Find(id);

            Accommodation.Approved = true;
            await Context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}