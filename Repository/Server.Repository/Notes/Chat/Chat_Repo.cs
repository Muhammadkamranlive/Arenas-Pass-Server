using Server.Core;
using Server.Domain;
using Microsoft.AspNetCore.Http;

namespace Server.Repository
{
    public class Chat_Repo : Repo<Chat>, IChat_Repo
    {
        private readonly ERPDb _db; private readonly IHttpContextAccessor _httpContextAccessor;
        public Chat_Repo(ERPDb db,IHttpContextAccessor httpContextAccessor) : base(db,httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor=httpContextAccessor;
        }
    }
}
