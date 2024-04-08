using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class GeneralTask_Service:Base_Service<GENERALTASK>, IGeneralTask_Service
    {
        public GeneralTask_Service(IUnitOfWork unitOfWork, IGeneralTask_Repo _Repo) : base(unitOfWork, _Repo)
        {

        }
    }
}
