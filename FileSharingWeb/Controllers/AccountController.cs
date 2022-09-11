using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FileSharingWeb.Resources;
using FileSharingWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace FileSharingWeb.Controllers
{

    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IStringLocalizer<AccountController> _localizer;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IStringLocalizer<AccountController> localizer)
        {
            _userManager = userManager;
            _localizer = localizer;
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
                ModelState.AddModelError("", _localizer.GetString("email_exist"));
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
        public async Task<IActionResult> Login(LoginVM loginVM, string returnUrl)
        {
            if (!ModelState.IsValid) return View(loginVM);

            var user = IsEmailExist(loginVM.Email);

            if (user == null)
            {
                ModelState.AddModelError("", _localizer.GetString("email_password_wrong"));
                return View(loginVM);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, loginVM.Password, true, true);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", _localizer.GetString("email_password_wrong"));

                return View(loginVM);
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        private IdentityUser IsEmailExist(string email)
        {
            return _userManager.Users.FirstOrDefault(u => u.Email == email);

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile(string name)
        {
            var user = await _userManager.Users.Where(u => u.UserName == name)
            .Select(u => new UserVM { Username = u.UserName, Email = u.Email })
            .FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }

            return View(user);

        }
    }
}