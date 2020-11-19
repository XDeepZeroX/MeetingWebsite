using MeetingWebsite.Models;
using MeetingWebsite.ModelsView.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingWebsite.ModelsView.Dialogs
{
    public class DialogView
    {
        public DialogView(Dialog dialog)
        {
            Id = dialog.Id;
            Title = dialog.Title;
            Users = dialog.DialogsUsers.Where(p => p.User != null)
                .Select(p => new UserView(p.User)).ToList();
            CreateUserId = dialog.CreateUserId;
            if (dialog.CreateUser != null)
                CreateUser = new UserView(dialog.CreateUser);
        }

        public int Id { get; set; }
        /// <summary>
        /// Название диалога
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Участники диалога
        /// </summary>
        public List<UserView> Users { get; set; } = new List<UserView>();

        /// <summary>
        /// Идентификатор создателя диалога
        /// </summary>
        public int CreateUserId { get; set; }

        /// <summary>
        /// Создатель диалога
        /// </summary>
        public UserView CreateUser { get; set; }
    }
}
