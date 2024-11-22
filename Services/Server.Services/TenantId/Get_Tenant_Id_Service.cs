using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Server.Services
{
    public class Get_Tenant_Id_Service: IGet_Tenant_Id_Service
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Get_Tenant_Id_Service
        (
            
          IHttpContextAccessor httpContextAccessor    
        )
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetTenantId()
        {
            var tenantId = Convert.ToInt32(_httpContextAccessor.HttpContext?.Items["CurrentTenant"]);
            return tenantId;
        }
    }
}
