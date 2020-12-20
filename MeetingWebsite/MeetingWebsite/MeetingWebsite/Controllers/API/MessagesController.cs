using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeetingWebsite.Models;
using MeetingWebsite.ModelsView.Messages;
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
    public class MessagesController : _BaseApiController
    {
        private readonly MessagesRepository _messagesRepository;
        private readonly DialogsRepository _dialogsRepository;
        public MessagesController(UsersRepository userRepository,
            MessagesRepository messagesRepository,
            DialogsRepository dialogsRepository) : base(userRepository)
        {
            _messagesRepository = messagesRepository;
            _dialogsRepository = dialogsRepository;
        }

        /// <summary>
        /// Возвращает список сообщений в диалоге
        /// </summary>
        /// <param name="dialogId">Идентификатор диалога</param>
        /// <param name="lastKey">Ключ сообщения, следом за которым необходимо отбирать выборку</param>
        /// <param name="count">Количество возвращаемых сообщений</param>
        /// <returns></returns>
        [HttpGet("{dialogId}")]
        public async Task<IActionResult> GetList(int dialogId, int lastKey = -1, int count = 1000)
        {
            var currentUserId = CurrentUserId();

            if (count < 0) count = 1000;

            var isAccess = (await _dialogsRepository.GetList()
                .Include(p => p.DialogsUsers)
                .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == dialogId))
                ?.DialogsUsers?.Any(p => p.UserId == currentUserId) ?? false;

            if (!isAccess)
                return StatusCode(404);

            var query = _messagesRepository.GetList()
                .Where(p => p.DialogId == dialogId);

            if (lastKey > 0)
                query = query.Where(p => p.Id < lastKey);


            var result = query.Take(count).ToList();

            return Ok(result.Select(p => new MessageView(p)).ToList());
        }


        /// <summary>
        /// Добавление нового сообщения
        /// </summary>
        /// <param name="dialogId">Идентификатор диалога</param>
        /// <param name="message">Тело сообщения</param>
        /// <returns>Возвращает идентификатор созданного сообщения</returns>
        [HttpPost("")]
        public async Task<IActionResult> Create(int dialogId, string message)
        {
            message = message?.Trim();
            if (string.IsNullOrEmpty(message))
                return StatusCode(400, "Сообщение не должно быть пустым");

            if (!await _dialogsRepository.Any(p => p.Id == dialogId))
                return StatusCode(404, "Диалог с данным идентификатором не найден");

            var userId = CurrentUserId();
            var messageDb = new Message()
            {
                Text = message,
                UserId = userId,
                DialogId = dialogId,
            };
            if (!await _messagesRepository.Add(messageDb))
                return StatusCode(500, "Не удалось добавить сообщение. Сообщите нам об этой ошибке");

            return Ok(messageDb.Id);
        }

        /// <summary>
        /// Изменение сообщения
        /// </summary>
        /// <param name="dialogId">Идентификатор диалога</param>
        /// <param name="messageId">Идентификатор сообщения</param>
        /// <param name="newText">Новое тело сообщения</param>
        [HttpPut("{dialogId}/{messageId}")]
        public async Task<IActionResult> Update(int dialogId, int messageId, string newText)
        {
            var message = await _messagesRepository.Find(messageId);
            if (message == null)
                return StatusCode(404, "Сообщение не найдено");

            var currentUserId = CurrentUserId();
            if (message.UserId != currentUserId)
                return StatusCode(401, "Изменить сообщение может только его создатель");

            if ((message.DateCreate - DateTime.Now).TotalMinutes > 15)
                return StatusCode(400, "Сообщения можно изменять не позже 15 минут после создания");

            message.Text = newText;
            if (!await _messagesRepository.Update(message))
                return StatusCode(500, "Не удалось изменить сообщение. Сообщите нам об этой ошибке");

            return Ok();
        }

        /// <summary>
        /// Удаление сообщения
        /// </summary>
        /// <param name="dialogId">Идентификатор диалога</param>
        /// <param name="messageId">Идентификатор сообщения</param>
        [HttpDelete("{dialogId}/{messageId}")]
        public async Task<IActionResult> Delete(int dialogId, int messageId)
        {
            var message = await _messagesRepository.Find(messageId);
            var currentUserId = CurrentUserId();
            if (message.UserId != currentUserId)
                return StatusCode(401, "Удалить сообщение может только его создатель");

            if ((message.DateCreate - DateTime.Now).TotalMinutes > 15)
                return StatusCode(400, "Сообщения можно удалять не позже 15 минут после создания");

            if (!await _messagesRepository.Remove(message))
                return StatusCode(500, "Не удалось удалить сообщение. Сообщите нам об этой ошибке");

            return Ok();
        }

        /// <summary>
        /// Удаляет список сообщений
        /// </summary>
        /// <param name="messagesId">Идентификаторы сообщений</param>
        /// <returns>200 - если успешно</returns>
        [HttpDelete("")]
        public async Task<IActionResult> DeleteRange(List<int> messagesId)
        {
            try
            {
                if (messagesId.Count == 0) return Ok();
                var currentUserId = CurrentUserId();


                var messages = await _messagesRepository.GetList()
                    .Where(p => messagesId.Contains(p.Id) && p.UserId == currentUserId)
                    .ToListAsync();

                if (await _messagesRepository.RemoveRange(messages))
                    return Ok();
                else
                    return StatusCode(500);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

    }
}
