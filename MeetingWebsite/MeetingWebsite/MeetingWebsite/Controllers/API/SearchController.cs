using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeetingWebsite.Extensions;
using MeetingWebsite.Models;
using MeetingWebsite.ModelsView.UserModels;
using MeetingWebsite.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeetingWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SearchController : _BaseController
    {
        public SearchController(UsersRepository userRepository) : base(userRepository)
        {
        }


        /// <summary>
        /// Поиск пользователей по параметрам
        /// </summary>
        /// <returns>Возвращает максимум 25 пользователей</returns>
        [HttpGet("")]
        public async Task<IActionResult> SerachUsers(string city, Sex sex, int minAge = 0, int maxAge = 100)
        {
            city = city?.Normilized();

            if (minAge < 0) minAge = 0;

            var users = _userRepository.GetList()
                            .Where(p => p.CityNormalize.Contains(city) && p.Sex == sex && p.Age >= minAge && p.Age <= maxAge)
                            .ToList()
                            .Select(p => new UserView(p))
                            .ToList();
            return Ok(users);
        }
    }
}
