using Server.Core;
using Server.Domain;
using Microsoft.AspNetCore.Http;

namespace Server.Repository
{
    public class Blog_Repo:Repo<BlogPage>, IBlog_Repo
    {
        private readonly ERPDb _db; 
        public Blog_Repo(ERPDb db, IHttpContextAccessor httpContextAccessor) : base(db,httpContextAccessor)
        {
            _db = db;
        }
    }
}
