using System;
using Server.Core;
using System.Linq;
using System.Text;
using Server.Domain;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Server.Repository
{
    public class Designation_Repo:Repo<Designations>, IDesignation_Repo
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Designation_Repo(ERPDb eRPDb,IHttpContextAccessor httpContextFactory):base(eRPDb, httpContextFactory)
        {
            _httpContextAccessor = httpContextFactory;
        }

        public override async Task<IEnumerable<Designations>> GetAll()
        {
            var tenantId = Convert.ToInt32(_httpContextAccessor.HttpContext?.Items["CurrentTenant"]);
            var filteredLogs = await base.GetAll();
            return filteredLogs.Where(log => log.TenantId == tenantId).ToList();
        }

        public override async Task<IEnumerable<Designations>> Find(Expression<Func<Designations, bool>> predicate)
        {
            var tenantId = Convert.ToInt32(_httpContextAccessor.HttpContext?.Items["CurrentTenant"]);
            var filteredLogs = await base.Find(predicate);
            return filteredLogs.Where(log => log.TenantId == tenantId).ToList();
        }
    }
}
