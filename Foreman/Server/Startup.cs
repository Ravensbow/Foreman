using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using Foreman.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Foreman.Shared.Data.Identity;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System;
using Foreman.Server.Utility;
using Foreman.Server.Authorization;
using Foreman.Shared.Services;
using Foreman.Server.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Foreman.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews()
                .AddJsonOptions(o=>
                    o.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
                );
            services.AddDbContext<ApplicationContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("SqlServer")));
            services.AddDefaultIdentity<UserProfile>(opts => opts.SignIn.RequireConfirmedAccount = true)
                .AddRoles<Role>()
                .AddEntityFrameworkStores<ApplicationContext>();
            services.AddIdentityServer()
                .AddApiAuthorization<UserProfile, ApplicationContext>(options => {
                    options.IdentityResources["openid"].UserClaims.Add("name");
                    options.ApiResources.Single().UserClaims.Add("name");
                    options.IdentityResources["openid"].UserClaims.Add("role");
                    options.ApiResources.Single().UserClaims.Add("role");
                    options.IdentityResources["openid"].UserClaims.Add("Institution");
                    options.ApiResources.Single().UserClaims.Add("Institution");
                    options.IdentityResources["openid"].UserClaims.Add("CourseManager");
                    options.ApiResources.Single().UserClaims.Add("CourseManager");
                    options.IdentityResources["openid"].UserClaims.Add("CategoryManager");
                    options.ApiResources.Single().UserClaims.Add("CategoryManager");
                })
                /*.AddDeveloperSigningCredential()*/;

            // Need to do this as it maps "role" to ClaimTypes.Role and causes issues
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("role");

            services.AddAuthentication()
                .AddIdentityServerJwt();
            services.AddHttpContextAccessor();
            services.AddSingleton<IAuthorizationHandler, InstitutionMemberRequirementHandler>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IAuthorizeService, AuthorizeService>();
            services.AddScoped<IPluginService, PluginService>();
            services.AddScoped<IFileService, FileService>();
            services.AddSingleton<Microsoft.AspNetCore.Mvc.Infrastructure.IActionDescriptorChangeProvider>(PluginActionDescriptorChangeProvider.Instance);
            services.AddSingleton(PluginActionDescriptorChangeProvider.Instance);
            //services.LoadPlugins(Configuration);
            services.AddSingleton(services);
            services.AddSingleton(Configuration);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });

            CreateRolesAndPowerUser(serviceProvider).GetAwaiter().GetResult();
        }

        private async Task CreateRolesAndPowerUser(IServiceProvider serviceProvider)
        {
            //initializing custom roles 
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<UserProfile>>();
            string[] roleNames = { "Admin", "Manager", "Member" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 1
                    roleResult = await RoleManager.CreateAsync(new Role(roleName));
                }
            }
            //Here you could create a super user who will maintain the web app
            var poweruser = new UserProfile
            {

                UserName = Configuration.GetSection("PowerUserCredentials").GetValue<string>("Username"),
                Email = Configuration.GetSection("PowerUserCredentials").GetValue<string>("Email"),
            };
            //Ensure you have these values in your appsettings.json file
            string userPWD = Configuration.GetSection("PowerUserCredentials").GetValue<string>("Password");
            var _user = await UserManager.FindByEmailAsync(poweruser.Email);

            if (_user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(poweruser, userPWD);
                if (createPowerUser.Succeeded)
                {
                    //here we tie the new user to the role
                    await UserManager.AddToRoleAsync(poweruser, "Admin");

                    var token = await UserManager.GenerateEmailConfirmationTokenAsync(poweruser);
                    await UserManager.ConfirmEmailAsync(poweruser, token);

                }
            }
        }
    }
}
