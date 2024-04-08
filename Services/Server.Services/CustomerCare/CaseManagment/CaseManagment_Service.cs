using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class CaseManagment_Service:Base_Service<Case>, ICaseManagment_Service
    {
        public CaseManagment_Service(IUnitOfWork unitOfWork, ICase_Repo _Repo) : base(unitOfWork, _Repo)
        {

        }
    }
}
