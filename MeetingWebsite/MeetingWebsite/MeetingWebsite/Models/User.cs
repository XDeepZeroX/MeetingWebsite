using MeetingWebsite.Extensions;
using MeetingWebsite.Helpers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MeetingWebsite.Models
{
    /// <summary>
    /// Учетная запись пользователя
    /// </summary>
    public class User : BaseEntity
    {
        /// <summary>
        /// Никнейм пользователя
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        public Sex Sex { get; set; }

        /// <summary>
        /// Возвраст
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Город проживания
        /// </summary>
        private string _city;
        public string City
        {
            get => _city;
            set
            {
                _city = value;
                CityNormalize = value.Normilized();
            }
        }
        /// <summary>
        /// Нормализация названия города
        /// </summary>
        public string CityNormalize { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Имя Фамилия пользователя
        /// </summary>
        public string FullName { get => $"{FirstName} {LastName}"; }

        /// <summary>
        /// Флаг, подтвержден ли телефонный номер
        /// </summary>
        public virtual bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Телефонный номер
        /// </summary>
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// ХЭШ от пароля
        /// </summary>
        public virtual string PasswordHash { get; set; }

        /// <summary>
        /// Флаг, подтвержденный ли Email
        /// </summary>
        public virtual bool EmailConfirmed { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// Краткая информация о пользователе
        /// </summary>
        public string BriefInformation { get; set; }

        /// <summary>
        /// Фотографии пользователя
        /// </summary>
        public virtual List<UserPhoto> Photos { get; set; } = new List<UserPhoto>();

        /// <summary>
        /// Диалоги в которых состоит пользователь
        /// </summary>
        public List<DialogsUsers> DialogsUsers { get; set; } = new List<DialogsUsers>();
    }

    public enum Sex
    {
        /// <summary>
        /// Парень 
        /// </summary>
        [Display(Name = "Парень")]
        Man = 0,

        /// <summary>
        /// Деушки 
        /// </summary>
        [Display(Name = "Девушка")]
        Girl = 1,


        /// <summary>
        /// Другое 
        /// </summary>
        [Display(Name = "Другое")]
        Other = 2,
    }
}
