using Server.Core;
using Server.Domain;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;

namespace Server.Repository
{
    public class Case_Repo:Repo<Case>, ICase_Repo
    {
        private readonly ERPDb _db; private readonly IHttpContextAccessor _httpContextAccessor;
        public Case_Repo(ERPDb eRP, IHttpContextAccessor httpContextAccessor) :base(eRP, httpContextAccessor) 
        {
            _db = eRP;   
            _httpContextAccessor = httpContextAccessor;
        }

        //public override async Task<IEnumerable<Case>> GetAll()
        //{
        //    var tenantId = Convert.ToInt32(_httpContextAccessor.HttpContext?.Items["CurrentTenant"]);
        //    var filteredLogs = await base.GetAll();
        //    return filteredLogs.Where(log => log.TenantId == tenantId).ToList();
        //}

        //public override async Task<IEnumerable<Case>> Find(Expression<Func<Case, bool>> predicate)
        //{
        //    var tenantId = Convert.ToInt32(_httpContextAccessor.HttpContext?.Items["CurrentTenant"]);
        //    var filteredLogs = await base.Find(predicate);
        //    return filteredLogs.Where(log => log.TenantId == tenantId).ToList();
        //}
    }
}
