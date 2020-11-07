using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MeetingWebsite.Models;
using MeetingWebsite.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeetingWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : _BaseController
    {
        private readonly UserPhotosRepository _photosRepository;
        IWebHostEnvironment _appEnvironment;

        public PhotoController(UsersRepository userRepository,
            UserPhotosRepository photosRepository,
            IWebHostEnvironment appEnvironment) : base(userRepository)
        {
            _photosRepository = photosRepository;
            _appEnvironment = appEnvironment;
        }

        /// <summary>
        /// Размещение фотографии в профиле пользователя
        /// </summary>
        /// <returns></returns>
        [HttpPost("")]
        public async Task<IActionResult> UploadPhoto(IFormFile photo)
        {
            try
            {
                if (photo == null)
                    return StatusCode(400);

                var userId = await CurrentUserId();


                var photoId = await _photosRepository.GetListWithDeleted()
                    .Where(p => p.UserId == userId).CountAsync() + 1;
                // путь к папке Files
                string path = $"/Photos/{userId}_user/{photoId}_{photo.FileName}";
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await photo.CopyToAsync(fileStream);
                }
                var file = new UserPhoto { UserId = userId, Path = path };
                await _photosRepository.Add(file);
                return Ok(path);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Не удалось добавить фотографию");
            }

        }
    }
}
