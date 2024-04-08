using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class Dependent_Service:Base_Service<Dependent>, IDependent_Service
    {
        public Dependent_Service(IUnitOfWork unitOfWork, IDependent_Repo _Repo) : base(unitOfWork, _Repo)
        {

        }
    }
}
