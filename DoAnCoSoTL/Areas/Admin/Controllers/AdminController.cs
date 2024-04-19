using DoAnCoSoTL.Models;
using DoAnCoSoTL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace DoAnCoSoTL.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class AdminController : Controller
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;
        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> CreateAdminAccount()
            {
                if (!await _roleManager.RoleExistsAsync("Admin"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                var user = new ApplicationUser
                {
                    FullName = "Thu Phương",
                    UserName = "Admin1@gmail.com",
                    Email = "Admin1@gmail.com"
                };

                var result = await _userManager.CreateAsync(user, "Abc@123");

                if (result.Succeeded)
                {
                    // Kiểm tra người dùng đã được tạo thành công
                    var createdUser = await _userManager.FindByEmailAsync("Admin1@gmail.com");
                    if (createdUser != null)
                    {
                        await _userManager.AddToRoleAsync(createdUser, "Admin");
                        return Content("Admin Account Created Successfully!");
                    }
                    else
                    {
                        return BadRequest("Failed to Create Admin Account: User not found");
                    }
                }
                else
                {
                    return BadRequest("Failed to Create Admin Account: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

        }
    }

