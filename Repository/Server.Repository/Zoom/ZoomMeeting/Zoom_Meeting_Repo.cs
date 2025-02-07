using Server.Core;
using Server.Domain;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;

namespace Server.Repository
{
    public class Zoom_Meeting_Repo : Repo<ZoomMeetings>, IZoom_Meeting_Repo
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public Zoom_Meeting_Repo(ERPDb dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {

            _contextAccessor = httpContextAccessor;
        }

        public override async Task<IEnumerable<ZoomMeetings>> GetAll()
        {
            var tenantId = Convert.ToInt32(_contextAccessor.HttpContext?.Items["CurrentTenant"]);
            var filteredLogs = await base.GetAll();
            return filteredLogs.Where(log => log.TenantId == tenantId).ToList();
        }

        public override async Task<IEnumerable<ZoomMeetings>> Find(Expression<Func<ZoomMeetings, bool>> predicate)
        {
            var tenantId = Convert.ToInt32(_contextAccessor.HttpContext?.Items["CurrentTenant"]);
            var filteredLogs = await base.Find(predicate);
            return filteredLogs.Where(log => log.TenantId == tenantId).ToList();
        }
    }
}
