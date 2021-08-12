using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Distribution.Controllers
{
    public class RolesController : Controller
    {
        public UserManager<IdentityUser> userManager;
        public RoleManager<IdentityRole> roleManager;

        public RolesController(UserManager<IdentityUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            userManager = _userManager;
            roleManager = _roleManager;
        }
        public async Task<IActionResult> Index()
        {
            IdentityRole adminRole = new IdentityRole { Name = "Admin"};
            IdentityRole userRole = new IdentityRole { Name = "User" };

            IdentityResult adminResult = await roleManager.CreateAsync(adminRole);
            IdentityResult userResult = await roleManager.CreateAsync(userRole);

            IdentityUser user1 = await userManager.FindByIdAsync("4c50f734-3abc-4b71-9ecc-4c90fe6accc9");
            adminResult = await userManager.AddToRoleAsync(user1, "Admin");

            IdentityUser user2 = await userManager.FindByIdAsync("77109523-7fa8-4710-ab9a-8fb4faaf06b7");
            userResult = await userManager.AddToRoleAsync(user2, "User");
            
            return View();
        }
    }
}
