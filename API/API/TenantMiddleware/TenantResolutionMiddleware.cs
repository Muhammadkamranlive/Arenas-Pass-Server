using System.Security.Claims;

namespace API.TenantMiddleware
{
    public class TenantResolutionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantResolutionMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Invoke(HttpContext context)
        {
            var userIdentity = context.User.Identity as ClaimsIdentity;
            if (userIdentity != null)
            {
                // Extract TenantId claim
                var tenantClaim = userIdentity.Claims.FirstOrDefault(c => c.Type == "TenantId");
                if (tenantClaim != null && int.TryParse(tenantClaim.Value, out int tenantId))
                {
                    context.Items["CurrentTenant"] = tenantId;
                }

                // Extract UserId claim
                var userIdClaim = userIdentity.Claims.FirstOrDefault(c => c.Type == "uid");
                if (userIdClaim != null)
                {
                    context.Items["CurrentUserId"] = userIdClaim.Value;
                }
            }

            await _next(context);
        }
    }
}
