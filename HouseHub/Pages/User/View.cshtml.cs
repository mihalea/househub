using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseHub.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HouseHub.Pages.User
{
    public class ViewModel : BasePageModel
    {
        [BindProperty] public IdentityUser FormUser { get; set; }

        public ViewModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager) : base(context, authorizationService, userManager)
        {
        }

        public async Task OnGetAsync(string id)
        {
            FormUser = await UserManager.FindByIdAsync(id);
        }
    }
}