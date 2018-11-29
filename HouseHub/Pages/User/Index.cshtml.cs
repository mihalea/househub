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
        [BindProperty] public IDictionary<string, IList<ApplicationUser>> Users { get; set; }
        [BindProperty] public IList<String> Roles { get; set; }
        [BindProperty] public IList<SelectListItem> RoleSelects{ get; set; }
        [BindProperty] public String Message { get; set; }

        private RoleManager<IdentityRole> RoleManager;

        public ManageModel(
            ApplicationDbContext context, 
            IAuthorizationService authorizationService, 
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager) : base(context, authorizationService, userManager)
        {
            RoleManager = roleManager;
            Users = new Dictionary<string, IList<ApplicationUser>>();
        }

        public async Task OnGetAsync()
        {
            var currentUser = await UserManager.GetUserAsync(User);
            var rawUsers = await UserManager.Users
                .Where(u => u.Id != currentUser.Id)
                .ToListAsync();


            RoleSelects = await RoleManager.Roles
                .Select(r => new SelectListItem {Text = r.Name, Value = r.Name})
                .ToListAsync();

            Roles = await RoleManager.Roles
                .Select(r => r.Name)
                .ToListAsync();

            foreach (var role in RoleManager.Roles)
            {
                var users = await UserManager.GetUsersInRoleAsync(role.Name);
                users.Remove(currentUser);

                Users[role.Name] = users;
            }
            
        }

        public async Task<IActionResult> OnPostAsync(String user, String Roles)
        {
            var exists = await RoleManager.RoleExistsAsync(Roles);
            var appUser = await UserManager.FindByIdAsync(user);
            if (exists && user != null)
            {
                var currentRoles = await UserManager.GetRolesAsync(appUser);
                await UserManager.RemoveFromRolesAsync(appUser, currentRoles);
                await UserManager.AddToRoleAsync(appUser, Roles);
                TempData["Message"] = "Successfully changed " + appUser.Name + "'s role to " + Roles;
            }
            else
            {
                TempData["Message"] = "Error: Failed to change role";
            }


            return RedirectToPage("/User/Index");
        }
    }
}