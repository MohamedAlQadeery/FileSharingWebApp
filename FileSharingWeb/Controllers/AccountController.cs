using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FileSharingWeb.Extensions;
using FileSharingWeb.Models;
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
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IStringLocalizer<AccountController> _localizer;
        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IStringLocalizer<AccountController> localizer)
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
            var user = new AppUser()
            {
                UserName = registerVM.Username,
                Email = registerVM.Email,
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName
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
            Response.Cookies.Delete("username");
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
            //store full name in cookies 
            //StoreUsernameInCookies();
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        private AppUser IsEmailExist(string email)
        {
            return _userManager.Users.FirstOrDefault(u => u.Email == email);
        }
        public IActionResult ExternalLogin(string provider)
        {
            var props = _signInManager.ConfigureExternalAuthenticationProperties(provider, "/Account/ExternalLoginCallBack");
            return Challenge(props, provider);
        }


        public async Task<IActionResult> ExternalLoginCallBack()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                TempData["failed_login"] = "Login error";
                return RedirectToAction("Index", "Home");
            }
            //login using AspNetUserLogins table
            var exLoginResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true);
            //if login failed it means its new account 
            if (!exLoginResult.Succeeded)
            {
                var user = new AppUser
                {
                    Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                    UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                    FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                    LastName = info.Principal.FindFirstValue(ClaimTypes.Surname)
                };
                var createUserResult = await _userManager.CreateAsync(user);
                if (createUserResult.Succeeded)
                {
                    var userLoginsInfoResult = await _userManager.AddLoginAsync(user, info);
                    if (userLoginsInfoResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, true, info.LoginProvider);
                    }
                }
                else
                {
                    // if login failed delete the created user
                    await _userManager.DeleteAsync(user);
                }
                return RedirectToAction("Login");
            }
            // if login succeeded
            return RedirectToAction("Index", "Home");
        }



    }
}