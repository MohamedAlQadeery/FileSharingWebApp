using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IStringLocalizer<SharedResource> _stringLocalizer;

        public ProfileController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IStringLocalizer<SharedResource> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _signInManager = signInManager;
            _userManager = userManager;

        }
        [HttpGet]
        [Route("Profile")]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var userVm = new UserVM
            {
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return View(userVm);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Profile")]
        public async Task<IActionResult> Profile(UserVM userVm)
        {
            if (!ModelState.IsValid) return View(userVm);
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = userVm.FirstName;
            user.LastName = userVm.LastName;
            user.UserName = userVm.Username;

            var updatedResult = await _userManager.UpdateAsync(user);
            if (!updatedResult.Succeeded)
            {
                foreach (var error in updatedResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(userVm);
            }

            // refresh signin to apply changes in cookies
            await _signInManager.RefreshSignInAsync(user);
            TempData["success_message"] = _stringLocalizer["success_message"].Value;
            return RedirectToAction("Profile");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(UserVM userVm)
        {
            if (!ModelState.IsValid) return View(userVm);
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = userVm.FirstName;
            user.LastName = userVm.LastName;
            user.UserName = userVm.Username;

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            return RedirectToAction("Profile");

        }




    }
}