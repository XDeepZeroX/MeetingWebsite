using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeetingWebsite.ModelsView.UserModels;
using MeetingWebsite.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeetingWebsite.Controllers
{
    public class UserController : BaseController
    {
        public UserController(UsersRepository userRepository) : base(userRepository)
        {
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(int id)
        {
            var user = await _userRepository.GetList()
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(p => p.Id == id);

            return View(user);
        }
    }
}
