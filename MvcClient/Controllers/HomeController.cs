using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MvcClient.Controllers
{
    public class HomeController: Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> UserInfo()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var idToken = await HttpContext.GetTokenAsync("id_token");
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            var _accessToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            var _idToken = new JwtSecurityTokenHandler().ReadJwtToken(idToken);

            var claim = HttpContext.User.Claims.ToList();

            var userName = claim.Find(x => x.Type == "name");
            var userRole = claim.Find(x => x.Type == ClaimTypes.Role);

            ViewData["UserName"] = userName?.Value;
            ViewData["UserRole"] = userRole?.Value;

            return View("UserInfo");
        }

        public IActionResult Logout()
        {
            return SignOut("Cookie", "oidc");
        }

        public IActionResult Login()
        {
            return RedirectToAction("UserInfo");
        }
    }
}
