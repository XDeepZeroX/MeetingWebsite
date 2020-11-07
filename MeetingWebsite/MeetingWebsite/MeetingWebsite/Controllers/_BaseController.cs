using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeetingWebsite.Models;
using MeetingWebsite.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeetingWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class _BaseController : ControllerBase
    {
        protected readonly UsersRepository _userRepository;
        public _BaseController(UsersRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Возвращает Email текущего пользователя
        /// </summary>
        /// <returns></returns>
        protected string CurrentEmail()
        {
            return User?.Claims?.FirstOrDefault(p => p.Type == "Email")?.Value;
        }

        /// <summary>
        /// Возвращает текущего пользователя
        /// </summary>
        /// <returns></returns>
        protected async Task<User> CurrentUser()
        {
            var emailUser = CurrentEmail();
            if (emailUser == null)
                return null;

            var user = await _userRepository.FirstOrDefault(p => p.Email == emailUser);
            return user;
        }

        protected async Task<int> CurrentUserId()
        {
            var emailUser = CurrentEmail();
            if (emailUser == null)
                return 0;

            var userId = await _userRepository.GetList()
                .Where(p => p.Email == emailUser)
                .Select(p => p.Id)
                .FirstOrDefaultAsync();
            return userId;

        }
    }
}
