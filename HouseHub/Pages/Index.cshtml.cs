using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseHub.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HouseHub.Pages
{
    public class IndexModel : BasePageModel
    {
        private IHostingEnvironment _environment;

        public IndexModel(
            IHostingEnvironment environment,
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager) : base(context, authorizationService, userManager)
        {
            _environment = environment;
        }

        public void OnGet()
        {

        }
    }
}
