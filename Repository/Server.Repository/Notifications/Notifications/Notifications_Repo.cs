using Server.Core;
using Server.Domain;
using Microsoft.AspNetCore.Http;

namespace Server.Repository
{
    public class Notifications_Repo:Repo<NOTIFICATIONS>, INotifications_Repo
    {
        private readonly ERPDb _db;
        public Notifications_Repo(ERPDb db, IHttpContextAccessor httpContextAccessor) :base(db, httpContextAccessor) 
        {
            _db = db;
        }
    }
}
