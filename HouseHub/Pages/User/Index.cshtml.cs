using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseHub.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HouseHub.Pages.User
{
    public class ManageModel : BasePageModel
    {
        [BindProperty] public IList<SelectListItem> Users { get; set; }

        public ManageModel(
            ApplicationDbContext context, 
            IAuthorizationService authorizationService, 
            UserManager<IdentityUser> userManager) : base(context, authorizationService, userManager)
        {
        }

        public async Task OnGetAsync()
        {
            Users = await UserManager.Users.Select(u => new SelectListItem {Text = u.UserName, Value = u.Id}).ToListAsync();
        }

        public IActionResult OnPost(String Users)
        {
            return RedirectToPage("View", new { id = Users });
        }
    }
}