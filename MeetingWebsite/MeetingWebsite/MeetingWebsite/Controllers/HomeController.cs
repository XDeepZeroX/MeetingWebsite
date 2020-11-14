using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MeetingWebsite.Models;
using Microsoft.AspNetCore.Authorization;

namespace MeetingWebsite.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return Redirect("/Profile");
            return View();
        }


        [HttpGet]
        public IActionResult Registration() => View();
    }
}
