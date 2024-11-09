using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class Tenants_Service:Base_Service<ArenasTenants>, ITenants_Service
    {
        public Tenants_Service(IUnitOfWork unitOfWork ,ITenants_Repo tenants_Repo):base(unitOfWork,tenants_Repo)
        {
            
        }
    }
}
