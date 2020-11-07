using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingWebsite.Models
{
    public class Dialog : BaseEntity
    {
        /// <summary>
        /// Название диалога
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Участники диалога
        /// </summary>
        public List<DialogsUsers> DialogsUsers { get; set; } = new List<DialogsUsers>();

        /// <summary>
        /// Идентификатор создателя диалога
        /// </summary>
        [ForeignKey("CreateUser")]
        [System.Text.Json.Serialization.JsonIgnore]
        [JsonIgnore]
        public int CreateUserId { get; set; }

        /// <summary>
        /// Создатель диалога
        /// </summary>
        public User CreateUser { get; set; }

        public virtual List<Message> Messages { get; set; } = new List<Message>();
    }

    /// <summary>
    /// Вспомогательная сущность 
    /// для связи многие ко многим
    /// </summary>
    public class DialogsUsers
    {
        public DialogsUsers() { }
        public DialogsUsers(int userId, int dialogId)
        {
            UserId = userId;
            DialogId = dialogId;
        }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Dialog")]
        public int DialogId { get; set; }
        public Dialog Dialog { get; set; }

    }
}
