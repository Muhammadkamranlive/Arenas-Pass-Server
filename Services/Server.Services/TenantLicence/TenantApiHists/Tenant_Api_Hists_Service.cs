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
    public class Tenant_Api_Hists_Service : Base_Service<TenantApiHitsHistory>, ITenant_Api_Hits_Service
    {
        private readonly ITenant_Api_Hits_Repo iRepo;
        public Tenant_Api_Hists_Service
        (
            IUnitOfWork unitOfWork,
            ITenant_Api_Hits_Repo genericRepository
        ) : base(unitOfWork, genericRepository)
        {
            iRepo = genericRepository;
        }
    }
}
