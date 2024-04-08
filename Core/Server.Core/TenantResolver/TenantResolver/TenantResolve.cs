using System;
using System.Linq;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Server.Core
{
    public sealed class TenantResolve 
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TenantResolve(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public string GetUserIdFromToken()
        {
            var userIdentity = _httpContextAccessor?.HttpContext?.User.Identity as ClaimsIdentity;
            var tenantClaim  = userIdentity?.Claims.FirstOrDefault(c => c.Type == "uid");
            if (tenantClaim != null && tenantClaim.Value!=null)
            {
                return tenantClaim.Value;
            }
            return "uid";
        }

        public int GetTenantId()
        {
            if (_httpContextAccessor?.HttpContext?.User?.Identity is ClaimsIdentity userIdentity)
            {
                var tenantClaim = userIdentity.Claims.FirstOrDefault(c => c.Type == "TenantId");
                if (tenantClaim != null && int.TryParse(tenantClaim.Value, out int tenantId))
                {
                    return tenantId;
                }
            }
            return 1;
        }
    }
}
