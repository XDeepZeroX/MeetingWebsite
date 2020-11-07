using MeetingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingWebsite.Repositories
{
    public class UserPhotosRepository : BaseRepository<UserPhoto>
    {
        public UserPhotosRepository(DBContext db) : base(db)
        {
        }
    }
}
