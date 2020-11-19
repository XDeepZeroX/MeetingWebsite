using MeetingWebsite.Helpers;
using MeetingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingWebsite.ModelsView.UserModels
{
    public class UpdateUserView
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public Sex Sex { get; set; }
        public string PhoneNumber { get; set; }
        public string Nickname { get; set; }
        public string City { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool Validate(out string errors)
        {
            errors = "";

            if (string.IsNullOrEmpty(NewPassword) || NewPassword.Length < 6)
                errors += "Длина нового пароля меньше 6 символов\n";

            if (string.IsNullOrEmpty(PhoneNumber))
                errors += "Не указан номер телефона\n";

            if (string.IsNullOrEmpty(Nickname) || Nickname.Length < 3)
                errors += "Длина ника меньше 3 символов\n";

            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
                errors += "Имя и фамилия являются обязательными полями\n";

            if (string.IsNullOrEmpty(City))
                errors += "Не указан город проживания\n";

            return errors == "";
        }

        public void UpdateDbModel(User user)
        {
            user.PasswordHash = MD5Helper.GetMD5Hash(NewPassword);
            user.Sex = Sex;
            user.PhoneNumber = PhoneNumber;
            user.City = City;
            user.Nickname = Nickname;
            user.FirstName = FirstName;
            user.LastName = LastName;
        }
    }
}
