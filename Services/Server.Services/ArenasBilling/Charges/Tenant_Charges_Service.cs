using System;
using Server.UOW;
using Server.Core;
using System.Linq;
using System.Text;
using Server.Domain;
using Server.Repository;
using Server.BaseService;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services
{
    public class Tenant_Charges_Service : Base_Service<TenantCharges>, ITenant_Charges_Service
    {
        public Tenant_Charges_Service
        (
            IUnit_Of_Work_Repo unitOfWork, 
            ITenant_Charges_Repo repo
        ) : base(unitOfWork, repo)
        {
        }
    }
}
