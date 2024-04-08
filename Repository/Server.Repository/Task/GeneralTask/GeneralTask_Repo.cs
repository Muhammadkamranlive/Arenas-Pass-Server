using Server.Core;
using Server.Domain;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;

namespace Server.Repository
{
    public class GeneralTask_Repo:Repo<GENERALTASK>, IGeneralTask_Repo
    {
        private readonly ERPDb _Db; private readonly IHttpContextAccessor _httpContextAccessor;
        public GeneralTask_Repo(ERPDb db, IHttpContextAccessor httpContextAccessor) :base(db, httpContextAccessor) 
        {
            _Db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task<IEnumerable<GENERALTASK>> GetAll()
        {
            var tenantId = Convert.ToInt32(_httpContextAccessor.HttpContext?.Items["CurrentTenant"]);
            var filteredLogs = await base.GetAll();
            return filteredLogs.Where(log => log.TenantId == tenantId).ToList();
        }

        public override async Task<IEnumerable<GENERALTASK>> Find(Expression<Func<GENERALTASK, bool>> predicate)
        {
            var tenantId = Convert.ToInt32(_httpContextAccessor.HttpContext?.Items["CurrentTenant"]);
            var filteredLogs = await base.Find(predicate);
            return filteredLogs.Where(log => log.TenantId == tenantId).ToList();
        }
    }
}
