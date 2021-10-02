using Front_Back.Areas.AdminPanel.ViewModels;
using Front_Back.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using sun.security.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Front_Back.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class AccountController : Controller
    {
        private UserManager<AppUser> _userManager;
      
        private SignInManager<AppUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager,RoleManager<IdentityRole>roleManager,SignInManager<AppUser> signInManager)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            signInManager = _signInManager;
        }

      

        public IActionResult Index()
        {
            AdminRegisterVM vm = new AdminRegisterVM();
            vm.Roles = new List<RoleVM>();
            foreach (var role in Enum.GetNames(typeof(Helper.Helper.Roles)))
            {
               vm.Roles.Add(new RoleVM
                {
                    Name = role
                });
            }
            return View(vm);
       
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(AdminRegisterVM register)
        {
            AdminRegisterVM vm = new AdminRegisterVM();
            vm.Roles = new List<RoleVM>();
            foreach (var role in Enum.GetNames(typeof(Helper.Helper.Roles)))
            {
                vm.Roles.Add(new RoleVM
                {
                    Name = role
                });
            }
            return View(vm);
            if (!ModelState.IsValid) return View(register);
            AppUser user = new AppUser
            {
                UserName=register.Username,
                Email=register.Email
            };
            IdentityResult result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);

                }
                return View(register);
            }
            await _userManager.AddToRoleAsync(user,Helper.Helper.Roles.Member.ToString());
            return RedirectToAction("Index", "Home",new { area=""});
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(AdminLoginVM login)
        {
            if (!ModelState.IsValid) return View(login);
            AppUser user = await _userManager.FindByNameAsync(login.Username);
            if (user == null)
            {
                ModelState.AddModelError("","Username or password is not correct" );
                return View(login);
                
            
            }
            if ((await _userManager.GetRolesAsync(user))[0] == Helper.Helper.Roles.SuperAdmin.ToString() ||
                    (await _userManager.GetRolesAsync(user))[0]== Helper.Helper.Roles.Admin.ToString())
                {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, login.Password, false, false);
                

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Username or password is not correct");
                        return View(login);


                    }
                    return RedirectToAction("Index", "Dashboard");
                }
            else
            {
                ModelState.AddModelError("", "Username or password is not correct");
                return View(login);

            }

        }
        //public async Task CreateRoles()
        //{
        //    await _roleManagerync(new IdentityRole(Helper.Helper.Roles.SuperAdmin.ToString()));
        //}
    }
}
