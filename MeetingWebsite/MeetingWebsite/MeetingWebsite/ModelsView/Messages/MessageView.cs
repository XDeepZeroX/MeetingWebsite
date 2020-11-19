using MeetingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingWebsite.ModelsView.Messages
{
    public class MessageView
    {
        public MessageView(Message message)
        {
            Id = message.Id;
            UserId = message.UserId;
            Text = message.Text;
            DateCreate = message.DateCreate;
            DateCreateUnix = message.DateCreateUnix;
        }

        public int Id { get; set; }

        /// <summary>
        /// Создатель сообщения
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Содержимое сообщения
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Дата создания сообщения
        /// </summary>
        public long DateCreateUnix { get; set; }

        public DateTime DateCreate
        {
            get => DateTime.UnixEpoch.AddSeconds(DateCreateUnix);
            set => DateCreateUnix = (int)((value - DateTime.UnixEpoch).TotalSeconds);
        }
    }
}
