using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace api.Service
{
    public static class AppRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";

    }
    
    public class SeedService
    {
        private readonly IServiceProvider _sp;
        private readonly ILogger<SeedService> _logger;
    
        public SeedService(IServiceProvider sp, ILogger<SeedService> logger)
        {
        _sp = sp;
        _logger = logger;
        }

        public async Task SeedAsync()
        {
            using var scope = _sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            await db.Database.MigrateAsync();

            await EnsureRoleAsync(roleManager, AppRoles.Admin, _logger);
            await EnsureRoleAsync(roleManager, AppRoles.User, _logger);

            const string adminEmail = "admin@testmail.com";
            const string adminPassword = "AdMinPass!23";

            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin is null)
            {
                admin = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var create = await userManager.CreateAsync(admin, adminPassword);
                if (!create.Succeeded)
                {
                    _logger.LogError("Admin create failed: {Errors}",
                        string.Join(", ", create.Errors.Select(e => e.Description)));
                    return;
                }
            }

            if (!await userManager.IsInRoleAsync(admin, AppRoles.Admin))
            {
                var addRole = await userManager.AddToRoleAsync(admin, AppRoles.Admin);
                if (!addRole.Succeeded)
                {
                    _logger.LogError("Assign Admin role failed : {Errors}",
                        string.Join(", ", addRole.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                _logger.LogInformation("Admin user ensured and in Admin role");
            }
        }


        private async Task EnsureRoleAsync(RoleManager<IdentityRole> rm,
        string roleName, ILogger logger)
        {
            if (!await rm.RoleExistsAsync(roleName))
            {
                var res = await rm.CreateAsync(new IdentityRole(roleName));
                if (!res.Succeeded)
                {
                    logger.LogError("Create role {Role} failed: {Errors}",
                        roleName, string.Join(", ", res.Errors.Select(e => e.Description)));
                    return;
                }
            }
        }
    }
}
