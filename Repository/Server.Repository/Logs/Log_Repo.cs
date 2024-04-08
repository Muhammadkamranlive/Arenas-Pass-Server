using Server.Core;
using Server.Domain;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;

namespace Server.Repository
{
    public class Log_Repo:Repo<AdminLogs>, ILog_Repo
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public Log_Repo(ERPDb eRPDb, IHttpContextAccessor httpContextAccessor) :base(eRPDb, httpContextAccessor)
        {
            
            _contextAccessor = httpContextAccessor;
        }


        public override async Task<IEnumerable<AdminLogs>> GetAll()
        {
            var tenantId      = Convert.ToInt32(_contextAccessor.HttpContext?.Items["CurrentTenant"]);
            if(tenantId != 4) 
            {
                var filteredLogs = await base.GetAll();
                return filteredLogs.Where(log => log.TenantId == tenantId).ToList();
            }
            var filteredLogs1 = await base.GetAll();
            return filteredLogs1.ToList();
        }

        public override async Task<IEnumerable<AdminLogs>> Find(Expression<Func<AdminLogs, bool>> predicate)
        {
            var tenantId     = Convert.ToInt32(_contextAccessor.HttpContext?.Items["CurrentTenant"]);
            if(tenantId != 4) 
            {
                var filteredLogs = await base.Find(predicate);
                return filteredLogs.Where(log => log.TenantId == tenantId).ToList();
            }

            var filteredLogs1 = await base.Find(predicate);
            return filteredLogs1.ToList();
        }
    }
}
