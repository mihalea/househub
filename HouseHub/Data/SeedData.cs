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

                try
                {
                    var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@soton.ac.uk");
                    await EnsureRole(serviceProvider, adminID, Constants.AdminRole);

                    var officerID = await EnsureUser(serviceProvider, testUserPw, "officer@soton.ac.uk");
                    await EnsureRole(serviceProvider, officerID, Constants.OfficerRole);

                    var landlordID = await EnsureUser(serviceProvider, testUserPw, "lord@soton.ac.uk");
                    await EnsureRole(serviceProvider, landlordID, Constants.LandlordRole);

                    Random random = new Random();
                    for (int i = 0; i < 50; i++)
                    {
                        var userID = await EnsureUser(serviceProvider, testUserPw, "user" + random.Next(100,999)  + "@soton.ac.uk");
                        await EnsureRole(serviceProvider, userID, Constants.DefaultRole);
                    }

                    SeedDatabase(context, adminID);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
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
                    ShortDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam.",
                    Description = "   An opportunity to purchase this end terrace ex-local authority property located in the village of Rhosgadfan. The deceptively spacious accommodation is fully double glazed and has been extended by the present owners. In brief the property benefits from entrance hall, living room, lounge and kitchen to the ground floor and four bedrooms and a bathroom to the first floor landing. The property does require some modernisation work. Outside: Front and rear lawn garden with a timber shed and coal shed. Magnificent view from the back garden towards neighbouring countryside and the coast.  " +
                                  "     " +
                                  " \n  Extended end terrace ex-local authority property  " +
                                  "   Two reception rooms and kitchen  " +
                                  "   four bedrooms and bathroom  " +
                                  "   in need of some modernisation work  " +
                                  "   fully double glazed  " +
                                  "   front and rear garden  " +
                                  "   views  " +
                                  "     " +
                                  "     " +
                                  "     " +
                                  "   Entrance hall x .  " +
                                  "     " +
                                  "   Living room 10\"5\" x 11\"10\"(3.18m x 3.6m).  "  + 
                                "     " +
                                "  Outside x . Front and rear lawn garden. To the rear there are distant coastal views.   ",
                    ImagePath = "/Uploads/pusheen.png",
                    OwnerID = adminID
                });
            }

            context.SaveChanges();
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
            string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = UserName,
                    Email = UserName,
                    PhoneNumber = "07492334876",
                    Name = "Ben Dover"
                };
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

            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByIdAsync(uid);

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }

    }
}
