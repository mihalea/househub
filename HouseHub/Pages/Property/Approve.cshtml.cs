﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseHub.Data;
using HouseHub.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HouseHub.Pages.Property
{
    public class ApproveModel : BasePageModel
    {
        [BindProperty] public IList<Accommodation> Accommodation { get; set; }

        public ApproveModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager) : base(context, authorizationService, userManager)
        {
        }

        
        public async Task OnGetAsync()
        {
            Accommodation = await Context.Accommodation.Where(a => a.Approved == false).ToListAsync();
        }
    }
}