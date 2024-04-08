using Server.Core;
using Server.Domain;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;

namespace Server.Repository
{
    public class AssetManager_Repo:Repo<Asset>, IAssetManager_Repo
    {
        private readonly ERPDb eRPDb; private readonly IHttpContextAccessor _httpContextAccessor;
        public AssetManager_Repo(ERPDb db, IHttpContextAccessor httpContextAccessor) :base(db, httpContextAccessor) 
        {
            eRPDb = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task<IEnumerable<Asset>> GetAll()
        {
            var tenantId = Convert.ToInt32(_httpContextAccessor.HttpContext?.Items["CurrentTenant"]);
            var filteredLogs = await base.GetAll();
            return filteredLogs.Where(log => log.TenantId == tenantId).ToList();
        }

        public override async Task<IEnumerable<Asset>> Find(Expression<Func<Asset, bool>> predicate)
        {
            var tenantId = Convert.ToInt32(_httpContextAccessor.HttpContext?.Items["CurrentTenant"]);
            var filteredLogs = await base.Find(predicate);
            return filteredLogs.Where(log => log.TenantId == tenantId).ToList();
        }
    }
}
