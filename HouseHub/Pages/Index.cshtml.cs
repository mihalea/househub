using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseHub.Data;
using HouseHub.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

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

        public IList<Accommodation> Accommodation { get; set; }

        public async Task OnGetAsync()
        {
            Accommodation = await Context.Accommodation.Where(a => a.Pending == false && a.Approved == true).ToListAsync();
        }
    }
}
