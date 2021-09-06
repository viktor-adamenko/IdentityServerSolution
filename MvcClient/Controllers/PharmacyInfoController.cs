using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcClient.Models;
using MvcClient.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MvcClient.Controllers
{
    [Authorize]
    public class PharmacyInfoController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly IHttpClientFactory _httpClientFactory;

        public PharmacyInfoController(ITokenService tokenService, IHttpClientFactory httpClientFactory)
        {
            _tokenService = tokenService;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            using var httpClient = _httpClientFactory.CreateClient();

            //var accessToken = await _tokenService.GetToken("identityserver");
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            httpClient.SetBearerToken(accessToken);

            var result = await httpClient.GetAsync("https://localhost:44340/api/pill/getPills");
            if(result.IsSuccessStatusCode)
            {
                var model = await result.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<Pill>>(model);

                return View(data);
            } 
            else
            {
                throw new Exception("Unable to get content");
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminAccess()
        {
            return View();
        }
    }
}
