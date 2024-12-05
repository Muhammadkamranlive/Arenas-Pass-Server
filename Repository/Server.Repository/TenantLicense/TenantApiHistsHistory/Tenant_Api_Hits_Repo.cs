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
    public class Tenant_Api_Hits_Repo : Repo<TenantApiHitsHistory>, ITenant_Api_Hits_Repo
    {

        public Tenant_Api_Hits_Repo
        (
            ERPDb coursecontext, IHttpContextAccessor httpContextAccessor
        ) : base(coursecontext, httpContextAccessor)
        {
        }
    }
}
