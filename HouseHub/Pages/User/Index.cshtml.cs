using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HouseHub.Data;
using HouseHub.Model;
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
        [BindProperty] public IList<SelectListItem> Roles { get; set; }

        private RoleManager<IdentityRole> RoleManager;

        public ManageModel(
            ApplicationDbContext context, 
            IAuthorizationService authorizationService, 
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager) : base(context, authorizationService, userManager)
        {
            RoleManager = roleManager;
        }

        public async Task OnGetAsync()
        {
            var rawUsers = await UserManager.Users.ToListAsync();

            Users = new List<SelectListItem>();
            foreach (var user in rawUsers)
            {
                var roles = await UserManager.GetRolesAsync(user);
                StringBuilder builder = new StringBuilder();
                builder.Append(" [");
                bool hasItems = false;
                foreach (var role in roles)
                {
                    if (hasItems)
                    {
                        builder.Append(", ");
                    }
                    builder.Append(role);
                    hasItems = true;
                }

                builder.Append("]");
                Users.Add(new SelectListItem{Text = user.UserName + builder.ToString(), Value = user.Id});
            }
            Roles = await RoleManager.Roles
                .Select(r => new SelectListItem {Text = r.Name, Value = r.Name})
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(String Users, String Roles)
        {
            var exists = await RoleManager.RoleExistsAsync(Roles);
            var user = await UserManager.FindByIdAsync(Users);
            if (exists && user != null)
            {
                var currentRoles = await UserManager.GetRolesAsync(user);
                await UserManager.RemoveFromRolesAsync(user, currentRoles);
                await UserManager.AddToRoleAsync(user, Roles);
                
            }

            return RedirectToPage("/User/Index");
        }
    }
}