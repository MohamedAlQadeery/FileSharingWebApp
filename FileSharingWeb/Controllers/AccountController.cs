using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FileSharingWeb.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FileSharingWeb.Controllers
{

    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);
            if (IsEmailExist(registerVM.Email) != null)
            {
                ModelState.AddModelError("", "Email is already taken");
                return View(registerVM);

            }

            var user = new IdentityUser()
            {
                UserName = registerVM.Username,
                Email = registerVM.Email,

            };

            var result = await _userManager.CreateAsync(user, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(registerVM);
            }

            await _signInManager.SignInAsync(user, true);
            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            var user = IsEmailExist(loginVM.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Email does not exist !");
                return View(loginVM);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, loginVM.Password, true, true);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Email/Password is wrong !");

                return View(loginVM);
            }


            return RedirectToAction("Index", "Home");
        }

        private IdentityUser IsEmailExist(string email)
        {
            return _userManager.Users.FirstOrDefault(u => u.Email == email);

        }
    }
}