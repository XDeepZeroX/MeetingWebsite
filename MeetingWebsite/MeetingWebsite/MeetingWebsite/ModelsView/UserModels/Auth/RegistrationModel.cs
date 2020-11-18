using MeetingWebsite.Helpers;
using MeetingWebsite.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingWebsite.ModelsView.Auth
{
    public class RegistrationModel
    {
        private string _email;
        public string Email { get => _email; set => _email = value.Trim(); }

        public string Password { get; set; }
        public Sex Sex { get; set; }

        public string PhoneNumber { get; set; }

        /// <summary>
        /// Никнейм пользователя
        /// </summary>
        public string Nickname { get; set; }
        public string City { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public bool Validate(out string errors)
        {
            errors = "";

            if (Email == null && RegexHelpers.Email.Match(Email).Value != Email)
                errors += "Не корректный Email\n";

            if (string.IsNullOrEmpty(Password) || Password.Length < 6)
                errors += "Длина пароля меньше 6 символов\n";

            if (string.IsNullOrEmpty(PhoneNumber))
                errors += "Не указан номер телефона\n";

            if (string.IsNullOrEmpty(Nickname) || Nickname.Length < 3)
                errors += "Длина ника меньше 3 символов\n";

            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
                errors += "Имя и фамилия являются обязательными полями\n";

            if (string.IsNullOrEmpty(City))
                errors += "Не указан город проживания\n";

            if (Age < 18)
                errors += "Вам нет 18 лет";
            else if (Age > 100)
                errors += "Указан не корректный возраст";

            return errors == "";
        }

        public User ToDbModel()
        {
            return new User()
            {
                Email = Email,
                PasswordHash = MD5Helper.GetMD5Hash(Password),
                Sex = Sex,
                PhoneNumber = PhoneNumber,
                City = City,
                Nickname = Nickname,
                FirstName = FirstName,
                LastName = LastName,
                Age = Age
            };
        }
    }
}
