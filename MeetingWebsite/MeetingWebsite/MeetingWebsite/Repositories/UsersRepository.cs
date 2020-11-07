using MeetingWebsite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingWebsite.Repositories
{
    public class UsersRepository : BaseRepository<User>
    {
        public UsersRepository(DBContext db) : base(db)
        {
        }

        /// <summary>
        /// Проверяет существование пользователей в БД по идентификаторам
        /// </summary>
        /// <param name="usersId">Идентификаторы пользователей</param>
        /// <returns>Идентификаторы пользователей которых нет в БД</returns>
        public async Task<List<int>> IsExists(List<int> usersId)
        {
            try
            {
                usersId = usersId.Distinct().ToList();

                var successUsersId = await GetList().Where(p => usersId.Contains(p.Id)).Select(p => p.Id).ToListAsync();

                return usersId.Except(successUsersId).ToList();

            }
            catch (Exception ex)
            {
            }
            return new List<int>();
        }
    }
}
