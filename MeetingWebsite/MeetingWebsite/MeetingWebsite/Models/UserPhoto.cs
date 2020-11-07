using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingWebsite.Models
{
    /// <summary>
    /// Фотография пользователя
    /// </summary>
    public class UserPhoto : BaseEntity
    {
        /// <summary>
        /// Путь к фотографии в файловом хранилище
        /// </summary>
        public string Path { get; set; }



        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public User User { get; set; }
    }
}