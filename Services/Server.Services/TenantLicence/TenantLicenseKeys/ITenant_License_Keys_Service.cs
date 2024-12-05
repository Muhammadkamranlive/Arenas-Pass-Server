using System;
using System.Linq;
using System.Text;
using Server.Core;
using Server.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services
{
    public interface ITenant_License_Keys_Service:IBase_Service<TenantLicenes>
    {
        Task<TenantLicenes> CreateOrUpdateKeysForTenant();
        Task<TenantLicenes> GetApiKeys();
    }
}
