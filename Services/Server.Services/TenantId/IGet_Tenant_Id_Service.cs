using System;
using System.Linq;
using System.Text;
using Server.Core;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services
{
    public interface IGet_Tenant_Id_Service
    {
        int GetTenantId();   
    }
}
