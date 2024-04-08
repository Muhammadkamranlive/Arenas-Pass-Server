using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustomAuthorizeAttribute : TypeFilterAttribute
    {
        public CustomAuthorizeAttribute(string permission) : base(typeof(CustomAuthorizeFilter))
        {
            Arguments = new object[] { permission };
        }
    }

    public class CustomAuthorizeFilter : IAuthorizationFilter
    {
        private readonly string _permission;

        public CustomAuthorizeFilter(string permission)
        {
            _permission = permission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            
            var hasPermission = CheckPermission(context);

            if (!hasPermission)
            {
                context.Result = new ForbidResult();
            }
        }

        private bool CheckPermission(AuthorizationFilterContext context)
        {
         
            var claims = context.HttpContext.User.Claims;
            var hasPermission = claims.Any(c => c.Type == "Permission" && c.Value.Contains(_permission));
            return hasPermission;
        }
    }

}
