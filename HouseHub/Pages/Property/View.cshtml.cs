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
    public class ViewModel : BasePageModel
    {
        [BindProperty] public Accommodation Accommodation { get; set; }
        [BindProperty] public ApplicationUser Owner { get; set; }

        public ViewModel(
            ApplicationDbContext context, 
            IAuthorizationService authorizationService, 
            UserManager<ApplicationUser> userManager) : base(context, authorizationService, userManager)
        {
        }

        public async Task OnGetAsync(int id)
        {
            Accommodation = Context.Accommodation.Find(id);
            if(Accommodation != null) {
                Owner = await UserManager.FindByIdAsync(Accommodation.OwnerID);
            }
        }
    }
}