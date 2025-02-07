using System;
using System.Linq;
using System.Text;
using Server.Core;
using Server.Domain;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Server.Repository
{
    public class Tenant_License_Keys_Repo : Repo<TenantLicenes>, ITenant_License_Keys_Repo
    {
        public Tenant_License_Keys_Repo
        (
            ERPDb coursecontext, IHttpContextAccessor httpContextAccessor
        ) : base(coursecontext, httpContextAccessor)
        {
        }
    }
}
