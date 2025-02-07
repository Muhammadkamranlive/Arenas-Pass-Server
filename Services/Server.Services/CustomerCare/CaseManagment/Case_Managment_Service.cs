using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class Case_Managment_Service:Base_Service<Case>, ICase_Managment_Service
    {
        public Case_Managment_Service(IUnit_Of_Work_Repo unitOfWork, ICase_Repo _Repo) : base(unitOfWork, _Repo)
        {

        }
    }
}
