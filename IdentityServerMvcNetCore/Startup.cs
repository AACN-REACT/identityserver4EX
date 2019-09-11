using IdentityServerMvcNetCore.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServerMvcNetCore
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
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));


            //services.AddDefaultIdentity<IdentityUser>()
            //.AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();

            // Configure Asp.Net Identity Core
            this.ConfigureAspNetIdentity(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Configure Identity Server 4
            this.ConfigureIdentityServer(services);
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
            app.UseCookiePolicy();

            // Configure CORS policy for Identity Server 4
            app.UseCors("default cors policy");

            // Removed per Identity Server 4
            //app.UseAuthentication();
            app.UseIdentityServer();

            app.UseMvc(
            //    routes =>
            //{
            //    routes.MapRoute(
            //        name: "identity",
            //        template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            //}
            );
        }

        private void ConfigureAspNetIdentity(IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });
        }

        private void ConfigureIdentityServer(IServiceCollection services)
        {
            services.AddIdentityServer(options =>
            {
                options.UserInteraction.LoginUrl = "/Identity/Account/Login";
                options.UserInteraction.LogoutUrl = "/Identity/Account/Logout";
                //options.UserInteraction.LogoutIdParameter = "logoutId";
                ////options.UserInteraction
            })
                .AddDeveloperSigningCredential()
                .AddInMemoryPersistedGrants()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddTestUsers(Config.GetUsers())
                .AddAspNetIdentity<IdentityUser>();
            ;

            services.AddCors(options =>
            {
                options.AddPolicy("default cors policy", policy =>
                {
                    policy.WithOrigins("*")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          ;
                });
            });

        }
    }
}