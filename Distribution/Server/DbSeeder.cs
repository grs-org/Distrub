using Distribution.Models;
using Distribution.Server;
using Distribution.Server.Helpers;
using Distribution.Shared.Constant;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace Distribution
{
    public class DatabaseSeeder : IAppInitialiser
    {
        private readonly ILogger<DatabaseSeeder> _logger;
        private readonly Unicontext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public DatabaseSeeder(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            Unicontext db,
            ILogger<DatabaseSeeder> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _logger = logger;
        }

        public void Initialize()
        {
            AddAdministrator();
            _db.SaveChanges();
        }

        private void AddAdministrator()
        {
            Task.Run(async () =>
            {
                //Check if Role Exists
                var adminRole = new AppRole(AdminConstants.AdminRole);
                var adminRoleInDb = await _roleManager.FindByNameAsync(AdminConstants.AdminRole);
                if (adminRoleInDb == null)
                {
                    await _roleManager.CreateAsync(adminRole);
                    adminRoleInDb = await _roleManager.FindByNameAsync(AdminConstants.AdminRole);
                    _logger.LogInformation("Seeded Administrator Role.");
                }
                //Check if User Exists
                var superUser = new AppUser
                {
                    Email = AdminConstants.SuperAdminEmail,
                    UserName = "SuperAdmin",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                };
                var superUserInDb = await _userManager.FindByEmailAsync(superUser.Email);
                if (superUserInDb == null)
                {
                    var seeded =  await _userManager.CreateAsync(superUser, AdminConstants.DefaultPassword);
                    if (seeded.Succeeded)
                    {
                        _logger.LogInformation("Seeded Default SuperAdmin User.");
                    }
                    else
                    {
                        foreach (var error in seeded.Errors)
                        {
                            _logger.LogError(error.Description);
                        }

                    }
                    var result = await _userManager.AddToRoleAsync(superUser, AdminConstants.AdminRole);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Seeded Default SuperAdmin User.");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            _logger.LogError(error.Description);
                        }

                    }
                }
                //foreach (var permission in Permissions.GetRegisteredPermissions())
                //{
                //    if (!permission.Contains("Logistics.Home"))
                //    {
                //        await _roleManager.AddPermissionClaim(adminRoleInDb, permission);
                //    }
                //}
            }).GetAwaiter().GetResult();
        }


    }
}
