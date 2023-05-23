using Distribution.Client.Authentification;
using Distribution.Models;
using Distribution.Server.Services;
using Distribution.Shared;
using Distribution.Shared.Constant;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace Distribution.Server
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private readonly IConfiguration _configuration;

        public void ConfigureServices(IServiceCollection services)
        {


            services.AddDbContext<Unicontext>(options => options
                    .UseSqlServer(_configuration.GetConnectionString("DefaultConnection")))
                    .AddTransient<IAppInitialiser, DatabaseSeeder>();

            services.AddIdentity<AppUser, AppRole>()
                             .AddEntityFrameworkStores<Unicontext>()
                             .AddDefaultTokenProviders();

            services.AddControllersWithViews();
            services.AddAuthorizationCore(options => { RegisterPermissionClaims(options); });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("5dqtETK72ZU@Gs6w!^hqJf9cgHz")),
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            RoleClaimType = ClaimTypes.Role,
                            ClockSkew = TimeSpan.Zero
                            
                            
                        };
                        
                    });
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped<IUserServicecs, UserServicecs>();


        }
        private static void RegisterPermissionClaims(AuthorizationOptions options)
        {
            foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c =>
                         c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                {
                    options.AddPolicy(propertyValue.ToString() ?? string.Empty,
                        policy => policy.RequireClaim(Permissions.ClaimType, propertyValue.ToString()));
                }
            }
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
          

            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            else
            {
                app.UseWebAssemblyDebugging();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
           
            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles();


            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapBlazorHub();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            app.Initialize(_configuration);
        }
      


    }
}
