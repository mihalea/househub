using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseHub.Authorisation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HouseHub.Data;
using HouseHub.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.IO;
using HouseHub.Model;

namespace HouseHub
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });

            services.AddMvc()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AddPageRoute(
                        "/Certbot/Key",
                        "/.well-known/acme-challenge/{id}"
                    );
                    options.Conventions.AuthorizePage("/Property/Create",
                        new [] {Constants.AdminRole, Constants.LandlordRole});
                    options.Conventions.AuthorizePage("/Property/Index", 
                        new [] {Constants.AdminRole, Constants.LandlordRole});
                    options.Conventions.AuthorizePage("/Property/Review",
                        new [] {Constants.AdminRole, Constants.OfficerRole});
                    options.Conventions.AuthorizePage("/Property/Reject", 
                        new [] {Constants.AdminRole, Constants.OfficerRole});
                    options.Conventions.AuthorizePage("/Property/Delete",
                        new[] {Constants.AdminRole, Constants.LandlordRole});
                    options.Conventions.AuthorizePage("/User/Index",
                        new[] { Constants.AdminRole });
                    options.Conventions.AuthorizePage("/Certbot/Index",
                        new[] { Constants.AdminRole });

                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddScoped<IAuthorizationHandler, AdminAuthorizationHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/")),
                RequestPath = "/Uploads"
            });

            //app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
