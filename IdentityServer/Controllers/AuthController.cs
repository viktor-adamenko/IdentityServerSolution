using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IIdentityServerInteractionService _interactionService;

        public AuthController(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager,
            IIdentityServerInteractionService interactionService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _interactionService = interactionService;
        }

        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (string.IsNullOrEmpty(vm.ReturnUrl))
            {
                vm.ReturnUrl = "/";
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var result = await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, false, false);

            if (result.Succeeded)
            {
                return Redirect(vm.ReturnUrl);
            }
            else
            {
                ModelState.AddModelError("error", "Wrong password or login");
            }

            return View(vm);
        }

        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();
            var logoutResult = await _interactionService.GetLogoutContextAsync(logoutId);

            if (string.IsNullOrEmpty(logoutResult.PostLogoutRedirectUri))
            {
                return Redirect("/Auth/Login");
            }

            return Redirect(logoutResult.PostLogoutRedirectUri);
        }

        public IActionResult Register(string returnUrl)
        {
            return View(new RegisterViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = new IdentityUser(vm.Username);
            var result = await _userManager.CreateAsync(user, vm.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("error", error.Description);
                }
            }
            else
            {
                var rolesResult = await _userManager.AddToRoleAsync(user, vm.Role);

                if (rolesResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);

                    return Redirect(vm.ReturnUrl);
                }
            }

            return View(vm);
        }
    }
}
