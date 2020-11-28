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
using Microsoft.EntityFrameworkCore;

namespace MeetingWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SearchController : _BaseApiController
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

            var query = _userRepository.GetList()
                            .Include(p => p.Photos)
                            .Where(p => p.Id != CurrentUserId() && p.Sex == sex && p.Age >= minAge && p.Age <= maxAge);
            if (!string.IsNullOrEmpty(city))
                query = query.Where(p => p.CityNormalize.Contains(city));

            var users = (await query.Take(25)
                            .ToListAsync())
                            .Select(p => new UserView(p))
                            .ToList();
            return Ok(users);
        }
    }
}
