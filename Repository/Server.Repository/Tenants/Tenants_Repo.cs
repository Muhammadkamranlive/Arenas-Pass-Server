using Server.Core;
using Server.Domain;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;

namespace Server.Repository
{
    public class Tenants_Repo:Repo<ArenasTenants>, ITenants_Repo
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public Tenants_Repo(ERPDb eRPDb, IHttpContextAccessor httpContextAccessor) :base(eRPDb, httpContextAccessor)
        {
            _contextAccessor = httpContextAccessor;
        }
       
    }
}
