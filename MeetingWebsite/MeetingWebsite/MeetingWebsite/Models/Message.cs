using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingWebsite.Models
{
    public class Message : BaseEntity
    {
        public Message()
        {
            DateCreate = DateTime.UtcNow;
        }
        /// <summary>
        /// Создатель сообщения
        /// </summary>
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        /// <summary>
        /// Идентификатор диалога
        /// </summary>
        [ForeignKey("Dialog")]
        public int DialogId { get; set; }
        public Dialog Dialog { get; set; }

        /// <summary>
        /// Содержимое сообщения
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Дата создания сообщения
        /// </summary>
        public long DateCreateUnix { get; set; }

        [NotMapped]
        public DateTime DateCreate
        {
            get => new DateTime(DateCreateUnix * 1000);
            set => DateCreateUnix = value.Ticks / 10000;
        }
    }
}
