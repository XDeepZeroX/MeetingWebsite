using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeetingWebsite.Configs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeetingWebsite.Controllers
{
    [Authorize]
    public class DialogsController : Controller
    {
        
        public IActionResult Index(int? id = null)
        {
            return View(id);
        }
    }
}
