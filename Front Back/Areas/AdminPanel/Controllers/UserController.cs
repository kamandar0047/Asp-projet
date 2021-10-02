using Front_Back.Areas.AdminPanel.ViewModels;
using Front_Back.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Front_Back.Areas.AdminPanel.Controllers
{
    public class UserController : Controller
    {
        private UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            userManager = _userManager;
        }
        public async Task<IActionResult> IndexAsync()
        {
            List<AppUser> users = _userManager.Users.ToList();
            List<UserVM> userVMs = new List<UserVM>();
            foreach (AppUser user in users)
            {
                userVMs.Add(new UserVM
                {
                    Username = user.UserName,
                    Email=user.Email,
                Role= (await _userManager.GetRolesAsync(user))[0]
                });
            }
            return View(userVMs);
        }
    }
}
