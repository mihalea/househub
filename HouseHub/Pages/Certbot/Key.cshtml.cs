using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseHub.Data;
using HouseHub.Extensions;
using HouseHub.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace HouseHub.Pages
{
    public class KeyModel : BasePageModel
    {
        [BindProperty] public string Key { get; set; }

        private readonly IConfiguration Configuration;

        public KeyModel(
            ApplicationDbContext context, 
            IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration) : base(context, authorizationService, userManager)
        {
            Configuration = configuration;
        }

        public void OnGet(string url)
        {
            Key = Configuration["Certbot:Key"];
        }
    }
}