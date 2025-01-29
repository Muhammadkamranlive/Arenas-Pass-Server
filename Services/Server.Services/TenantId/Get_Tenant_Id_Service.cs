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

        public string GetUserId()
        {
            var tenantId = _httpContextAccessor.HttpContext?.Items["CurrentUserId"]?.ToString();
            return tenantId;
        }

        public string GetUserName()
        {
            var Name = _httpContextAccessor.HttpContext?.Items["Name"]?.ToString();
            return Name;
        }

        public string GetCompanyName() 
        {
            var Name = _httpContextAccessor.HttpContext?.Items["CompanyName"]?.ToString();
            return Name;
        }

        public string GetDesignation()
        {
            var Name = _httpContextAccessor.HttpContext?.Items["CompanyDesignation"]?.ToString();
            return Name;
        }
    }
}
