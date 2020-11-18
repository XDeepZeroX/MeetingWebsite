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
    public class _BaseApiController : ControllerBase
    {
        protected readonly UsersRepository _userRepository;
        public _BaseApiController(UsersRepository userRepository)
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

        protected int CurrentUserId()
        {
            return int.Parse(User.Claims.FirstOrDefault(p => p.Type == "Id").Value);
        }
    }
}
