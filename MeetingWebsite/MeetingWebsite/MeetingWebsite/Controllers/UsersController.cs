using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeetingWebsite.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeetingWebsite.Controllers
{
    [Authorize]
    public class UsersController : BaseController
    {
        public UsersController(UsersRepository userRepository) : base(userRepository)
        {
        }

        public IActionResult Index()
        {
            var users = _userRepository.GetList()
                .Include(p => p.Photos)
                .Where(p => p.Id != CurrentUserId())
                .Take(25)
                .ToList();
            return View(users);
        }
    }
}
