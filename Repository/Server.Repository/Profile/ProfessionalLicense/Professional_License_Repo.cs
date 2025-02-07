using Server.Core;
using Server.Domain;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;

namespace Server.Repository
{
    public class Professional_License_Repo:Repo<ProfessionalLicense>, IProfessional_License_Repo
    {
        private readonly ERPDb _db; private readonly IHttpContextAccessor _httpContextAccessor;
        public Professional_License_Repo(ERPDb db, IHttpContextAccessor httpContextAccessor) :base(db, httpContextAccessor) 
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task<IEnumerable<ProfessionalLicense>> GetAll()
        {
            var tenantId = Convert.ToInt32(_httpContextAccessor.HttpContext?.Items["CurrentTenant"]);
            var filteredLogs = await base.GetAll();
            return filteredLogs.Where(log => log.TenantId == tenantId).ToList();
        }

        public override async Task<IEnumerable<ProfessionalLicense>> Find(Expression<Func<ProfessionalLicense, bool>> predicate)
        {
            var tenantId = Convert.ToInt32(_httpContextAccessor.HttpContext?.Items["CurrentTenant"]);
            var filteredLogs = await base.Find(predicate);
            return filteredLogs.Where(log => log.TenantId == tenantId).ToList();
        }
    }
}
