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
    public class Tenant_Key_History_Service : Base_Service<TenantKeyHistory>, ITenant_Key_History_Service
    {
        private readonly ITenant_Key_History_Repo iRepo;
        public Tenant_Key_History_Service
        (
            IUnitOfWork unitOfWork, ITenant_Key_History_Repo genericRepository
        ) : base(unitOfWork, genericRepository)
        {
            iRepo = genericRepository;
        }
    }
}
