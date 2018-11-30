using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HouseHub.Data;
using HouseHub.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace HouseHub.Pages
{
    public class CertbotModel : BasePageModel
    {
        [BindProperty]
        [Required]
        [RegularExpression(".+\\..+")]
        public String Key { get; set; }
        public String CurrentKey { get; set; }

        private readonly IConfiguration Configuration;

        public CertbotModel(
            ApplicationDbContext context, 
            IAuthorizationService authorizationService, 
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration) : base(context, authorizationService, userManager)
        {
            Configuration = configuration;
        }

        public void OnGet()
        {
            var key = Configuration["Certbot:Key"];
            if (key == null)
            {
                CurrentKey = "No key set";
            }
            else
            {
                CurrentKey = key;
            }
        }

        public IActionResult OnPost(string Key)
        {
            var url = Key.Split(".")[0];

            Configuration["Certbot:Url"] = url;
            Configuration["Certbot:Key"] = Key;

            TempData["Message"] = "Key has been updated";
            return RedirectToPage("/Certbot/Index");
        }


    }
}