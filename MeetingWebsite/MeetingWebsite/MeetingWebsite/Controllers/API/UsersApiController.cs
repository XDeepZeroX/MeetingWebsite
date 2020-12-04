using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeetingWebsite.ModelsView.UserModels;
using MeetingWebsite.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeetingWebsite.Controllers
{
    [Route("api/Users")]
    [ApiController]
    [Authorize]
    public class UsersApiController : _BaseApiController
    {
        public UsersApiController(UsersRepository userRepository) : base(userRepository)
        {
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> UserInfo(int id)
        {
            var user = await _userRepository.GetList()
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (user == null)
                return StatusCode(404, "Пользователь не найден");

            return Ok(new UserView(user));
        }
    }
}
