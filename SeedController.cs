using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace homework_8.Controllers
{
    public class SeedController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public SeedController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Initialize()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await _roleManager.RoleExistsAsync("Employee"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Employee"));
            }
            var adminUser = await _userManager.FindByEmailAsync("admin@resources.com");
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = "admin@resources.com",
                    Email = "admin@resources.com",
                    EmailConfirmed = true 
                };
                var result = await _userManager.CreateAsync(adminUser, "Admin@123");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else
                { 
                    return Content($"❌ Failed to create admin user.");
                }
            }

            //Creating Employee User for test
            var employeeUser = await _userManager.FindByEmailAsync("employee@resources.com");
            if (employeeUser == null)
            {
                employeeUser = new IdentityUser
                {
                    UserName = "employee@resources.com",
                    Email = "employee@resources.com",
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(employeeUser, "Employee@123");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(employeeUser, "Employee");
                }
            }

            return Content(
                "✅Database seeded successfully!<br>);");
        }
    }
}