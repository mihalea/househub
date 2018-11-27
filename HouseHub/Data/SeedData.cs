using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseHub.Model;

namespace HouseHub.Data
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {

                // dotnet user-secrets set SeedUserPW <pw>


                var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@soton.ac.uk");
                await EnsureRole(serviceProvider, adminID, Constants.AdminRole);

                var officerID = await EnsureUser(serviceProvider, testUserPw, "officer@soton.ac.uk"); 
                await EnsureRole(serviceProvider, officerID, Constants.OfficerRole);

                var landlordID = await EnsureUser(serviceProvider, testUserPw, "lord@soton.ac.uk");
                await EnsureRole(serviceProvider, landlordID, Constants.LandlordRole);

                SeedDatabase(context, adminID);
            }
        }

        public static void SeedDatabase(ApplicationDbContext context, string adminID)
        {
            foreach (var entity in context.Accommodation)
            {
                context.Accommodation.Remove(entity);
            }

            for (var i = 0; i < 12; i++)
            {
                context.Accommodation.Add(new Accommodation
                {
                    Name = "Burgess Road " + new Random().Next(100),
                    Description = "The greatest place on earth can be found right here at our house",
                    ImagePath = "/Uploads/pusheen.png"
                });
            }

            context.SaveChanges();
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
            string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new IdentityUser { UserName = UserName };
                await userManager.CreateAsync(user, testUserPw);
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
            string uid, string role)
        {
            IdentityResult IR = null;
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByIdAsync(uid);

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }

    }
}
