using MeetingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingWebsite.Repositories
{
    public class MessagesRepository : BaseRepository<Message>
    {
        public MessagesRepository(DBContext db) : base(db)
        {
        }

        public override IQueryable<Message> GetList()
        {
            return base.GetList().OrderByDescending(p => p.DateCreateUnix);
        }

        public override IQueryable<Message> GetListWithDeleted()
        {
            return base.GetListWithDeleted().OrderByDescending(p => p.DateCreateUnix);
        }
    }
}
