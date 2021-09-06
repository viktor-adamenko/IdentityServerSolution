using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcClient.Controllers
{
    public class ErrorController : Controller
    {        
        [AllowAnonymous]
        [Route("Error")]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = 
                HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            ViewBag.ExceptionMessage = exceptionHandlerPathFeature.Error.Message;

            return View("Error");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
