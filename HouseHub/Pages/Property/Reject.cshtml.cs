using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Threading.Tasks;
using HouseHub.Data;
using HouseHub.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HouseHub.Pages.Property
{
    public class RejectModel : BasePageModel
    {
        [BindProperty] public Accommodation Accommodation { get; set; }

        [BindProperty][Required][StringLength(200, MinimumLength = 5)]
        public string Reason { get; set; }
        [BindProperty]
        public string Message { get; set; }

        public RejectModel(
            ApplicationDbContext context, 
            IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager) : base(context, authorizationService, userManager)
        {
        }

        public void OnGet(int id)
        {
            Accommodation = Context.Accommodation.Find(id);
        }

        public async Task<IActionResult> OnPostAsync(int id, String reason)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Accommodation = Context.Accommodation.Find(id);

            if (Accommodation != null)
            {
                Accommodation.Approved = false;
                Accommodation.Pending = false;
                Accommodation.Reason = reason;

                await Context.SaveChangesAsync();

                Message = "Application has been rejected";
                return Page();
            }

            Message = "An internal error occurred while trying to reject the application";
            return Page();
        }
    }
}