using MeetingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingWebsite.Repositories
{
    public class DialogsRepository : BaseRepository<Dialog>
    {
        public DialogsRepository(DBContext db) : base(db)
        {
        }


        public async Task<bool> AddUsers(List<DialogsUsers> users)
        {
            try
            {
                await _db.AddRangeAsync(users);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
